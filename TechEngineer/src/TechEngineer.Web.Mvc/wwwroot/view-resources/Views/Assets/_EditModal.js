(function ($) {
    var _assetService = abp.services.app.asset,
        l = abp.localization.getSource('TechEngineer'),
        _$modal = $('#AssetEditModal'),
        _$form = _$modal.find('form');

    function save() {
        if (!_$form.valid()) {
            return;
        }

        var asset = _$form.serializeFormToObject();
        debugger;
        asset.organizationId = $('.selected-organization').attr("id");
        asset.locationId = $('.location_dd').children(":selected").attr("id");

        abp.ui.setBusy(_$form);
        _assetService.update(asset).done(function () {
            _$modal.modal('hide');
            abp.notify.info(l('SavedSuccessfully'));
            abp.event.trigger('asset.edited', asset);
        }).always(function () {
            abp.ui.clearBusy(_$form);
        });
    }

    _$form.closest('div.modal-content').find(".save-button").click(function (e) {
        e.preventDefault();
        save();
    });

    _$form.find('input').on('keypress', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            save();
        }
    });

    _$modal.on('shown.bs.modal', function () {
        debugger;
        _$form.find('input[type=text]:first').focus();
        var date = document.getElementById("startWarrentyDate");
    });
})(jQuery);