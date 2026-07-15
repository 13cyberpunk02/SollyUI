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