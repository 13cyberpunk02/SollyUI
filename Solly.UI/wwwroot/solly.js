const clamp = (v, min, max) => Math.max(min, Math.min(max, v));
const KEY = 'solly-theme';
export function anchor(el, anchorEl) {
    if (!el || !anchorEl) return;

    // measure without flashing
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

    const below = vh - a.bottom - gap;
    const above = a.top - gap;
    const flip = p.height > below && above > below;

    const height = Math.min(p.height, flip ? above : below);
    const top = flip ? a.top - gap - height : a.bottom + gap;
    const left = clamp(a.left, 8, Math.max(8, vw - p.width - 8));

    el.style.top = `${Math.round(top)}px`;
    el.style.left = `${Math.round(left)}px`;
    el.style.minWidth = `${Math.round(a.width)}px`;
    el.style.maxHeight = `${Math.round(height)}px`;
    el.style.visibility = 'visible';
}

export function registerDismiss(root, dotnet) {
    const onPointerDown = (e) => {
        if (root && !root.contains(e.target)) dotnet.invokeMethodAsync('OnDismissAsync');
    };
    const onKeyDown = (e) => {
        if (e.key === 'Escape') dotnet.invokeMethodAsync('OnDismissAsync');
    };
    const onReflow = () => dotnet.invokeMethodAsync('OnDismissAsync');

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
    el?.focus?.({ preventScroll: true });
}

export function scrollItemIntoView(container, index) {
    const item = container?.querySelector(`[data-gidx="${index}"]`);
    item?.scrollIntoView({ block: 'nearest' });
}

export function setTheme(theme) {
    const resolved = theme === 'auto'
        ? (window.matchMedia('(prefers-color-scheme: light)').matches ? 'light' : 'dark')
        : theme;
    document.documentElement.setAttribute('data-solly-theme', resolved);
    try { localStorage.setItem(KEY, theme); } catch { }
}

export function getStoredTheme() {
    try { return localStorage.getItem(KEY); } catch { return null; }
}

export function autoGrow(el) {
    if (!el) return null;
    const resize = () => {
        el.style.height = 'auto';
        el.style.height = `${el.scrollHeight}px`;
    };
    resize();
    el.addEventListener('input', resize);
    return { dispose: () => el.removeEventListener('input', resize) };
}

export function portal(el) {
    if (!el || el.parentElement === document.body) return null;
    const home = el.parentElement;
    const next = el.nextSibling;
    document.body.appendChild(el);
    return {
        dispose: () => {
            try { home?.insertBefore(el, next); } catch { }
        }
    };
}

export function trapFocus(el, dotnet) {
    if (!el) return null;

    const prev = document.activeElement;
    const SEL = 'a[href],button:not([disabled]),textarea:not([disabled]),input:not([disabled]):not([type=hidden]),select:not([disabled]),[tabindex]:not([tabindex="-1"])';

    const items = () => Array.from(el.querySelectorAll(SEL)).filter(n => n.offsetParent !== null);

    const onKey = (e) => {
        if (e.key === 'Escape') { dotnet.invokeMethodAsync('OnEscapeAsync'); return; }
        if (e.key !== 'Tab') return;

        const list = items();
        if (list.length === 0) { e.preventDefault(); return; }

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
    (list[0] ?? el).focus({ preventScroll: true });

    return {
        dispose: () => {
            el.removeEventListener('keydown', onKey);
            try { prev?.focus?.({ preventScroll: true }); } catch { }
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