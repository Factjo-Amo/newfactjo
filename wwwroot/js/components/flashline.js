// FlashLine — حركة خبر واحد في كل نقرة، مع دعم RTL/لمس/إيقاف عند الهوفر
(function () {
    function initFlashLine(root) {
        console.log('FlashLine initialized', root);

        const track = root.querySelector('.flashline-track');
        const prevBtn = root.querySelector('.fl-prev');
        const nextBtn = root.querySelector('.fl-next');
        const items = () => Array.from(root.querySelectorAll('.fl-item'));

        let busy = false;
        let autoTimer = null;
        const AUTO = false;      // 🔧 فعّلها لاحقًا لو بدك تشغيل تلقائي
        const AUTO_MS = 4000;    // مدة الانتقال التلقائي
        const TRANSITION_MS = 300;

        function gapPx() {
            const g = getComputedStyle(track).gap || getComputedStyle(track).columnGap || "0px";
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
            busy = true;
            const step = itemStep();
            setTransition(true);
            track.style.transform = `translateX(-${step}px)`;

            const onEnd = () => {
                track.removeEventListener('transitionend', onEnd);
                // دوّر العنصر الأول للنهاية
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
            busy = true;
            const step = itemStep();
            // حضّر العنصر الأخير في البداية بدون أن يرى المستخدم قفزة
            setTransition(false);
            const all = items();
            const last = all[all.length - 1];
            if (last) track.insertBefore(last, all[0]);
            track.style.transform = `translateX(-${step}px)`;
            forceReflow();
            // ثم حرّك للموضع الطبيعي بصريًا
            setTransition(true);
            track.style.transform = 'translateX(0)';

            const onEnd = () => {
                track.removeEventListener('transitionend', onEnd);
                setTransition(false);
                busy = false;
            };
            track.addEventListener('transitionend', onEnd, { once: true });
        }

        // أزرار
        nextBtn && nextBtn.addEventListener('click', next);
        prevBtn && prevBtn.addEventListener('click', prev);

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
            const x = (e.touches ? e.touches[0].clientX : e.clientX);
            const dx = x - startX;
            // لا نسحب بصريًا لتبسيط، فقط نقرر الاتجاه عند الإفلات
        }
        function onPointerUp(e) {
            if (!dragging) return;
            dragging = false;
            const x = (e.changedTouches ? e.changedTouches[0].clientX : e.clientX);
            const dx = x - startX;
            const TH = 30; // عتبة
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

        // إعادة حساب عند تغيير المقاس
        window.addEventListener('resize', () => {
            setTransition(false);
            track.style.transform = 'translateX(0)';
        });

        // تشغيل تلقائي إن أردت
        startAuto();
    }

    // تهيئة كل FlashLine في الصفحة
    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('.flashline').forEach(initFlashLine);
    });
})();
