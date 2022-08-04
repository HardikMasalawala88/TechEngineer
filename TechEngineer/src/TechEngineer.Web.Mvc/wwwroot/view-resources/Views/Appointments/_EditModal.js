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
        debugger;
        var appointment = _$form.serializeFormToObject();
        if (abp.auth.grantedPermissions['Pages.Master.Organizations.Dropdown'] == true)//Super admin or Admin 
        {
            appointment.organizationId = $('.selected-organization').attr("id");
            appointment.locationId = $('.location_dd').children(":selected").attr("id");
        }
        else if (_userLogSession.organizationId) {
            appointment.organizationId = _userLogSession.organizationId;
            appointment.locationId = _userLogSession.locationId;
        }
        else {
            abp.message.error(abp.utils.formatString(l('Please select Organization before create from dropdown in header.')));
        }
        
        appointment.assetId = $('.asset_dd').children(":selected").attr("id");
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