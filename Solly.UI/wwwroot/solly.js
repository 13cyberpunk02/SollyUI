const clamp = (v, min, max) => Math.max(min, Math.min(max, v));
const KEY = 'solly-theme';

export function anchor(el, anchorEl) {
    if (!el || !anchorEl) return;

    el.style.visibility = 'hidden';
    el.style.position = 'fixed';
    el.style.top = '0px';
    el.style.left = '0px';
    el.style.maxHeight = '';

    const a = anchorEl.getBoundingClientRect();
    const p = el.getBoundingClientRect();
    const vw = window.innerWidth;
    const vh = window.innerHeight;
    const gap = 6;
    const margin = 8;

    const below = vh - a.bottom - gap - margin;
    const above = a.top - gap - margin;
    const flip = p.height > below && above > below;
    const space = flip ? above : below;

    let top = flip ? a.top - gap - p.height : a.bottom + gap;

    if (p.height > space) {
        el.style.maxHeight = `${Math.floor(space)}px`;
        top = flip ? margin : a.bottom + gap;
    }

    const left = clamp(a.left, margin, Math.max(margin, vw - p.width - margin));

    el.style.top = `${Math.round(top)}px`;
    el.style.left = `${Math.round(left)}px`;
    el.style.visibility = 'visible';
}

export function registerDismiss(root, panel, dotnet) {
    const inside = (t) => root?.contains(t) || panel?.contains(t);

    const onPointerDown = (e) => {
        if (!inside(e.target)) dotnet.invokeMethodAsync('OnDismissAsync');
    };
    const onKeyDown = (e) => {
        if (e.key === 'Escape') dotnet.invokeMethodAsync('OnDismissAsync');
    };
    const onReflow = (e) => {
        if (e?.target && panel?.contains(e.target)) return;
        dotnet.invokeMethodAsync('OnDismissAsync');
    };

    document.addEventListener('pointerdown', onPointerDown, true);
    document.addEventListener('keydown', onKeyDown, true);
    window.addEventListener('resize', onReflow);
    window.addEventListener('scroll', onReflow, true);

    return {
        dispose: () => {
            document.removeEventListener('pointerdown', onPointerDown, true);
            document.removeEventListener('keydown', onKeyDown, true);
            window.removeEventListener('resize', onReflow);
            window.removeEventListener('scroll', onReflow, true);
        }
    };
}

export function focusEl(el) {
    el?.focus?.({preventScroll: true});
}

export function scrollItemIntoView(container, index) {
    const item = container?.querySelector(`[data-gidx="${index}"]`);
    item?.scrollIntoView({block: 'nearest'});
}

export function setTheme(theme) {
    const resolved = theme === 'auto'
        ? (window.matchMedia('(prefers-color-scheme: light)').matches ? 'light' : 'dark')
        : theme;
    document.documentElement.setAttribute('data-solly-theme', resolved);
    try {
        localStorage.setItem(KEY, theme);
    } catch {
    }
}

export function getStoredTheme() {
    try {
        return localStorage.getItem(KEY);
    } catch {
        return null;
    }
}

export function autoGrow(el) {
    if (!el) return null;
    const resize = () => {
        el.style.height = 'auto';
        el.style.height = `${el.scrollHeight}px`;
    };
    resize();
    el.addEventListener('input', resize);
    return {dispose: () => el.removeEventListener('input', resize)};
}

export function portal(el) {
    if (!el || el.parentElement === document.body) return null;
    const home = el.parentElement;
    const next = el.nextSibling;
    document.body.appendChild(el);
    return {
        dispose: () => {
            try {
                if (!home) return;
                if (next && next.parentElement === home) {
                    home.insertBefore(el, next);
                } else {
                    home.appendChild(el);
                }
            } catch {
                el.remove();
            }
        }
    };
}

export function trapFocus(el, dotnet) {
    if (!el) return null;

    const prev = document.activeElement;
    const SEL = 'a[href],button:not([disabled]),textarea:not([disabled]),input:not([disabled]):not([type=hidden]),select:not([disabled]),[tabindex]:not([tabindex="-1"])';

    const items = () => Array.from(el.querySelectorAll(SEL)).filter(n => n.offsetParent !== null);

    const onKey = (e) => {
        if (e.key === 'Escape') {
            dotnet.invokeMethodAsync('OnEscapeAsync');
            return;
        }
        if (e.key !== 'Tab') return;

        const list = items();
        if (list.length === 0) {
            e.preventDefault();
            return;
        }

        const first = list[0];
        const last = list[list.length - 1];

        if (e.shiftKey && document.activeElement === first) {
            e.preventDefault();
            last.focus();
        } else if (!e.shiftKey && document.activeElement === last) {
            e.preventDefault();
            first.focus();
        } else if (!el.contains(document.activeElement)) {
            e.preventDefault();
            first.focus();
        }
    };

    el.addEventListener('keydown', onKey);

    const list = items();
    (list[0] ?? el).focus({preventScroll: true});

    return {
        dispose: () => {
            el.removeEventListener('keydown', onKey);
            try {
                prev?.focus?.({preventScroll: true});
            } catch {
            }
        }
    };
}

export function lockScroll() {
    const w = window.innerWidth - document.documentElement.clientWidth;
    const prevOverflow = document.body.style.overflow;
    const prevPad = document.body.style.paddingRight;
    document.body.style.overflow = 'hidden';
    if (w > 0) document.body.style.paddingRight = `${w}px`;
    return {
        dispose: () => {
            document.body.style.overflow = prevOverflow;
            document.body.style.paddingRight = prevPad;
        }
    };
}

export function anchorTip(el, anchorEl, placement) {
    if (!el || !anchorEl) return;

    el.style.visibility = 'hidden';
    el.style.position = 'fixed';
    el.style.top = '0px';
    el.style.left = '0px';

    const a = anchorEl.getBoundingClientRect();
    const p = el.getBoundingClientRect();
    const vw = window.innerWidth, vh = window.innerHeight;
    const gap = 8;

    const fits = {
        top: a.top - gap - p.height > 0,
        bottom: a.bottom + gap + p.height < vh,
        left: a.left - gap - p.width > 0,
        right: a.right + gap + p.width < vw,
    };

    let place = placement;
    if (!fits[place]) {
        const flip = {top: 'bottom', bottom: 'top', left: 'right', right: 'left'};
        if (fits[flip[place]]) place = flip[place];
        else place = Object.keys(fits).find(k => fits[k]) || placement;
    }

    let top, left;
    switch (place) {
        case 'top':
            top = a.top - gap - p.height;
            left = a.left + a.width / 2 - p.width / 2;
            break;
        case 'bottom':
            top = a.bottom + gap;
            left = a.left + a.width / 2 - p.width / 2;
            break;
        case 'left':
            top = a.top + a.height / 2 - p.height / 2;
            left = a.left - gap - p.width;
            break;
        default:
            top = a.top + a.height / 2 - p.height / 2;
            left = a.right + gap;
            break;
    }

    left = Math.max(8, Math.min(left, vw - p.width - 8));
    top = Math.max(8, Math.min(top, vh - p.height - 8));

    el.style.top = `${Math.round(top)}px`;
    el.style.left = `${Math.round(left)}px`;
    el.dataset.place = place;
    el.style.visibility = 'visible';
}

export function setPalette(h, s, l) {
    const r = document.documentElement;
    r.style.setProperty('--s-h', String(h));
    r.style.setProperty('--s-s', s + '%');
    r.style.setProperty('--s-l', l + '%');
    try {
        localStorage.setItem('solly-palette', JSON.stringify([h, s, l]));
    } catch {
    }
}

export function getStoredPalette() {
    try {
        const raw = localStorage.getItem('solly-palette');
        return raw ? JSON.parse(raw) : null;
    } catch {
        return null;
    }
}

export function registerHotkey(dotnet, combo) {
    const parts = combo.toLowerCase().split('+');
    const needMod = parts.includes('mod');
    const key = parts[parts.length - 1];

    const handler = (e) => {
        const mod = e.metaKey || e.ctrlKey;
        if (needMod && !mod) return;
        if (e.key.toLowerCase() !== key) return;
        e.preventDefault();
        dotnet.invokeMethodAsync('OnHotkeyAsync');
    };

    document.addEventListener('keydown', handler);
    return {
        dispose: () => document.removeEventListener('keydown', handler)
    };
}