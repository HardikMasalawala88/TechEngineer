(function ($) {
    var _appointmentService = abp.services.app.appointment,
        _userLogSession = ""
    l = abp.localization.getSource('TechEngineer'),
        _$modal = $('#AppointmentEditModal'),
        _$form = _$modal.find('form');

    if (!_userLogSession) {
        abp.services.app.session.getCurrentLoginInformations().then(data => {
            _userLogSession = data.user;
        });
    }

    function save() {
        if (!_$form.valid()) {
            return;
        }
        var appointment = _$form.serializeFormToObject();

        appointment.assetId = $('.asset_edit_dd').children(":selected").attr("id");
        appointment.userId = _userLogSession.id;

        abp.ui.setBusy(_$form);
        _appointmentService.update(appointment).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
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