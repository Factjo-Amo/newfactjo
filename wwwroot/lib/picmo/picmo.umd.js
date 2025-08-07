
// محاكاة محتوى مكتبة Picmo UMD
window.picmo = {
    createPopup: function () {
        return {
            addEventListener: function (event, callback) {
                console.log('📦 Emoji picker event linked');
            },
            toggle: function () {
                alert("📦 Emoji picker opened (mock)");
            }
        }
    }
};
