document.addEventListener('click', function (e) {
    const btn = e.target.closest('.share-btn');

    // فتح/إغلاق القائمة
    if (btn) {
        const wrap = btn.closest('.share');
        const menu = wrap.querySelector('.share-menu');
        const title = btn.dataset.title || document.title;
        const url = btn.dataset.url || location.href;

        // موبايل: Web Share API
        if (navigator.share && /Mobi|Android|iPhone|iPad/i.test(navigator.userAgent)) {
            navigator.share({ title, text: title, url }).catch(() => { });
            return;
        }

        const enc = encodeURIComponent;
        wrap.querySelector('.share-x').href =
            `https://twitter.com/intent/tweet?text=${enc(title)}&url=${enc(url)}`;
        wrap.querySelector('.share-fb').href =
            `https://www.facebook.com/sharer/sharer.php?u=${enc(url)}`;
        wrap.querySelector('.share-wa').href =
            `https://wa.me/?text=${enc(title + ' ' + url)}`;

        menu.hidden = !menu.hidden;
        btn.setAttribute('aria-expanded', String(!menu.hidden));
        e.preventDefault();
        return;
    }

    // إغلاق عند النقر خارج القائمة
    if (!e.target.closest('.share')) {
        document.querySelectorAll('.share-menu:not([hidden])')
            .forEach(m => m.hidden = true);
    }
});



// اغلاق بالق键 Esc
document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape') {
        document.querySelectorAll('.share-menu:not([hidden])')
            .forEach(m => m.hidden = true);
        document.querySelectorAll('.share-btn[aria-expanded="true"]')
            .forEach(b => b.setAttribute('aria-expanded', 'false'));
    }
});
