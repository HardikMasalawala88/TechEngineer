(function ($) {
    var _appointmentService = abp.services.app.appointment,
        l = abp.localization.getSource('TechEngineer'),
        _$modal = $('#AppointmentEditModal'),
        _$form = _$modal.find('form');

    function save() {
        if (!_$form.valid()) {
            return;
        }

        var appointment = _$form.serializeFormToObject();
        debugger;
        appointment.organizationId = $('.selected-organization').attr("id");
        appointment.locationId = $('.location_dd').children(":selected").attr("id");

        abp.ui.setBusy(_$form);
        _appointmentService.update(appointment).done(function () {
            _$modal.modal('hide');
            abp.notify.info(l('SavedSuccessfully'));
            abp.event.trigger('appointment.edited', appointment);
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