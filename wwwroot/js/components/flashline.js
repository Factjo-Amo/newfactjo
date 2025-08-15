// FlashLine — حركة عنصر واحد في كل نقرة، مع دعم RTL/لمس/إيقاف عند الهوفر
(function () {
    console.log('flashline.js LOADED (top)');
    function initFlashLine(root) {
        if (!root) return;
        if (root.dataset.flInit === '1') return; // منع التهيئة المكررة

        const track = root.querySelector('.flashline-track');
        const prevBtn = root.querySelector('.fl-prev');
        const nextBtn = root.querySelector('.fl-next');
        const items = () => Array.from(root.querySelectorAll('.fl-item'));

        // ⚠️ لا نعلن flInit=1 قبل التأكد من الجاهزية
        if (!track || items().length === 0) {
            console.warn('FlashLine: لا يوجد مسار أو عناصر داخل', root);
            return;
        }

        console.log('FlashLine initialized', root);

        let busy = false;
        let autoTimer = null;
        const AUTO = false;
        const AUTO_MS = 4000;
        const TRANSITION_MS = 300;

        function gapPx() {
            const g = getComputedStyle(track).gap || getComputedStyle(track).columnGap || '0px';
            return parseFloat(g) || 0;
        }

        function itemStep() {
            const first = items()[0];
            if (!first) return 0;
            const rect = first.getBoundingClientRect();
            return rect.width + gapPx();
        }

        function setTransition(on) {
            track.style.transition = on ? `transform ${TRANSITION_MS}ms ease` : 'none';
        }

        function forceReflow() { void track.offsetHeight; }

        function next() {
            if (busy) return;
            const step = itemStep();
            if (step <= 0) return;
            busy = true;
            setTransition(true);
            track.style.transform = `translateX(-${step}px)`;
            const onEnd = () => {
                track.removeEventListener('transitionend', onEnd);
                setTransition(false);
                track.style.transform = 'translateX(0)';
                const first = items()[0];
                if (first) track.appendChild(first);
                forceReflow();
                busy = false;
            };
            track.addEventListener('transitionend', onEnd, { once: true });
        }

        function prev() {
            if (busy) return;
            const step = itemStep();
            if (step <= 0) return;
            busy = true;

            setTransition(false);
            const all = items();
            const last = all[all.length - 1];
            if (last) track.insertBefore(last, all[0]);
            track.style.transform = `translateX(-${step}px)`;
            forceReflow();

            setTransition(true);
            track.style.transform = 'translateX(0)';
            const onEnd = () => {
                track.removeEventListener('transitionend', onEnd);
                setTransition(false);
                busy = false;
            };
            track.addEventListener('transitionend', onEnd, { once: true });
        }

        // أزرار (مع لوج للتشخيص) + منع السحب يبلع النقرة
        if (nextBtn) {
            nextBtn.addEventListener('mousedown', e => e.stopPropagation());
            nextBtn.addEventListener('click', () => { console.log('FL next'); next(); });
        }
        if (prevBtn) {
            prevBtn.addEventListener('mousedown', e => e.stopPropagation());
            prevBtn.addEventListener('click', () => { console.log('FL prev'); prev(); });
        }

        // تشغيل تلقائي (اختياري)
        function startAuto() {
            if (!AUTO || autoTimer) return;
            autoTimer = setInterval(next, AUTO_MS);
        }
        function stopAuto() {
            if (autoTimer) { clearInterval(autoTimer); autoTimer = null; }
        }

        // إيقاف عند الهوفر
        root.addEventListener('mouseenter', stopAuto);
        root.addEventListener('mouseleave', startAuto);

        // سحب باللمس/الماوس
        let startX = 0, dragging = false;
        function onPointerDown(e) {
            dragging = true;
            startX = (e.touches ? e.touches[0].clientX : e.clientX);
        }
        function onPointerMove(e) {
            if (!dragging) return;
        }
        function onPointerUp(e) {
            if (!dragging) return;
            dragging = false;
            const x = (e.changedTouches ? e.changedTouches[0].clientX : e.clientX);
            const dx = x - startX;
            const TH = 30;
            if (dx <= -TH) next();
            else if (dx >= TH) prev();
        }
        root.addEventListener('mousedown', onPointerDown);
        root.addEventListener('mousemove', onPointerMove);
        root.addEventListener('mouseup', onPointerUp);
        root.addEventListener('mouseleave', () => dragging = false);
        root.addEventListener('touchstart', onPointerDown, { passive: true });
        root.addEventListener('touchmove', onPointerMove, { passive: true });
        root.addEventListener('touchend', onPointerUp);

        // إعادة ضبط عند تغيير المقاس
        window.addEventListener('resize', () => {
            setTransition(false);
            track.style.transform = 'translateX(0)';
        });

        // ✅ الآن وبعد ما تأكدنا أن كل شيء جاهز وربطنا الأحداث، نعلن التهيئة
        root.dataset.flInit = '1';

        // بدء تلقائي (إن أردت)
        startAuto();
    }

    // === تهيئة كل FlashLine موجودة حاليًا أو ستظهر لاحقًا ===
    function initAllFlashLines() {
        document.querySelectorAll('.flashline').forEach(initFlashLine);
    }

    // نجرب كل مسارات التحميل + نكشف الدالة عالميًا للاحتياط
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initAllFlashLines);
    } else {
        initAllFlashLines();
    }
    window.addEventListener('load', initAllFlashLines);

    const mo = new MutationObserver(initAllFlashLines);
    mo.observe(document.documentElement, { childList: true, subtree: true });


    console.log('flashline.js BEFORE EXPOSE', typeof window.FlashLineInitAll);
    window.FlashLineInitAll = initAllFlashLines;
    console.log('flashline.js EXPOSED', typeof window.FlashLineInitAll);


    // اختياري: نوفر طريقة للاستدعاء اليدوي إن احتجتها
    window.FlashLineInitAll = initAllFlashLines;
})();
