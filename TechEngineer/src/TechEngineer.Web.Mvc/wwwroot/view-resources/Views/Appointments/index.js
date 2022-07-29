(function ($) {
    var _appointmentService = abp.services.app.appointment,
        l = abp.localization.getSource('TechEngineer'),
        _$modal = $('#AppointmentCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#AppointmentsTable');
    debugger
    var _$appointmentsTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _appointmentService.getAll,
            inputFilter: function () {
                return $('#AppointmentSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$appointmentsTable.draw(false)
            }
        ],
        responsive: {
            details: {
                type: 'column'
            }
        },
        columnDefs: [
            {
                targets: 0,
                className: 'control',
                defaultContent: '',
            },
            {
                targets: 1,
                data: 'asset.name',
                sortable: false
            },
            {
                targets: 2,
                data: 'requestDate',
                sortable: false
            },
            {
                targets: 3,
                data: 'location.address1',
                sortable: false
            },
            {
                targets: 4,
                data: 'organization.name',
                sortable: false
            },
            {
                targets: 5,
                data: 'status',
                sortable: false,
            },
            {
                targets: 6,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    debugger
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-appointment" data-appointment-id="${row.id}" data-toggle="modal" data-target="#AppointmentEditModal">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-appointment" data-appointment-id="${row.id}" data-appointment-name="${row.name}">`,
                        `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });

    _$form.validate({
        rules: {
            Password: "required",
            ConfirmPassword: {
                equalTo: "#Password"
            }
        }
    });

    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }
        debugger;
        var appointment = _$form.serializeFormToObject();
        appointment.organizationId = $('.organization-dropdown').children(":selected").attr("id");
        appointment.locationId = $('.location-dropdown').children(":selected").attr("id");
        appointment.assetId = $('.asset_dd').children(":selected").attr("id");

        abp.ui.setBusy(_$modal);
        _appointmentService.create(appointment).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            abp.notify.info(l('SavedSuccessfully'));
            $("#AppointmentCreateModal").hide();
            _$appointmentsTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });

    $(document).on('click', '.delete-appointment', function () {
        var appointmentId = $(this).attr("data-appointment-id");

        deleteAppointment(appointmentId);
    });

    function deleteAppointment(appointmentId) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDeleteAppointment')),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _appointmentService.delete({
                        id: appointmentId
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$appointmentsTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-appointment', function (e) {
        var appointmentId = $(this).attr("data-appointment-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Appointments/EditModal?appointmentId=' + appointmentId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#AppointmentEditModal div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    });

    $(document).on('click', 'a[id="CreateAppointmentsBtn"]', (e) => {
        if ($('.organization-dropdown')[0].value != "All Organization")
        {
            debugger;
            $("#AppointmentCreateModal").addClass('show');
            $("#AppointmentCreateModal").show();
            $('.nav-tabs a[href="#appointment-details"]').tab('show');
        }
        else {
            $("#AppointmentCreateModal").removeClass('show');
            abp.message.error(abp.utils.formatString(l('Please select Organization before create from dropdown in header.')));
        }
    });

    $(document).on('click', '.close', (e) => {
        $("#AppointmentCreateModal").hide();
        _$form.clearForm();
    });

    _$form.find('.close-button').on('click', (e) => {
        $("#AppointmentCreateModal").hide();
        _$form.clearForm();
    });

    document.getElementById('org_dropdown').addEventListener("change",
        function () {
            organization_selection();
        });

    function organization_selection() {
        let org_ = document.getElementById('org_dropdown');
        let organization_id = org_.options[org_.selectedIndex].id;
        location_(organization_id);
    }

    function location_(organization_id) {
        let locations = document.getElementById('loc_dropdown');
        for (var i = 0; i < locations.options.length; i++) {
            if (locations.options[i].dataset.bind == organization_id) {
                $("#loc_dropdown")[0].value = locations.options[i].value;
            }
            else {
                //$("#loc_dropdown")[0].remove(locations.options[i]);
            }
        }
        let location_id = locations.options[locations.selectedIndex].id;

        let asset_dd = document.getElementById('asset_data');
        for (var i = 0; i < asset_dd.options.length; i++)
        {
            if (asset_dd.options[i].dataset.bind == location_id) {
                $("#asset_data")[0].value = asset_dd.options[i].value;
            }
            else {
                //$("#loc_dropdown")[0].remove(locations.options[i]);
            }
        }

    }

    abp.event.on('appointment.edited', (data) => {
        _$appointmentsTable.ajax.reload();
    });

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        _$appointmentsTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$appointmentsTable.ajax.reload();
            return false;
        }
    });
})(jQuery);