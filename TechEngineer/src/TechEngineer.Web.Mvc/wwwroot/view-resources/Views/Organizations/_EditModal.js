(function ($) {
    var _organizationService = abp.services.app.organization,
        l = abp.localization.getSource('TechEngineer'),
        _$modal = $('#OrganizationEditModal'),
        _$form = _$modal.find('form');

    function save() {
        if (!_$form.valid()) {
            return;
        }

        var organization = _$form.serializeFormToObject();
        organization.location = {
            Address1: organization['AddressLine1'],
            Address2: organization['AddressLine2'],
            Landmark: organization['Landmark'],
            PostalCode: organization['PostalCode'],
            CityId: organization['City'],
            StateId: organization['State'],
            CountryId: organization['Country']
        };

        abp.ui.setBusy(_$form);
        _organizationService.update(organization).done(function () {
            _$modal.modal('hide');
            abp.notify.info(l('SavedSuccessfully'));
            abp.event.trigger('organization.edited', organization);
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
        _$form.find('input[type=text]:first').focus();
    });
})(jQuery);