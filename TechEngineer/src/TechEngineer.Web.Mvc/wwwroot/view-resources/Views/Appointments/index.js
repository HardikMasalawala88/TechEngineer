(function ($) {
    var defaultGuid = "00000000-0000-0000-0000-000000000000";
    var _appointmentService = abp.services.app.appointment,
        _userLogSession = ""
    l = abp.localization.getSource('TechEngineer'),
        _$modal = $('#AppointmentCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#AppointmentsTable');
    if (!_userLogSession) {
        abp.services.app.session.getCurrentLoginInformations().then(data => {
            _userLogSession = data.user;
        });
    }

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
                data: 'user.emailAddress',
                sortable: false
            },
            {
                targets: 3,
                data: 'requestDate',
                sortable: false,
                render: function (data, type, row) {
                    if (type === "sort" || type === "type") {
                        return data;
                    }
                    return moment(data).format("YYYY-MM-DD");
                }
            },
            {
                targets: 4,
                data: 'remarks',
                sortable: false
            },
            {
                targets: 5,
                data: 'location.address1',
                sortable: false
            },
            {
                targets: 6,
                data: 'organization.name',
                sortable: false
            },
            {
                targets: 7,
                data: 'status',
                sortable: false,
            },
            {
                targets: 8,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    if (abp.auth.grantedPermissions['Pages.Appointments.Delete'] == true) {
                        return [
                            `   <button type="button" class="btn btn-sm bg-secondary edit-appointment" data-appointment-id="${row.id}" data-toggle="modal" data-target="#AppointmentEditModal">`,
                            `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                            '   </button>',
                            `   <button type="button" class="btn btn-sm bg-danger delete-appointment" data-appointment-id="${row.id}" data-appointment-name="${row.name}">`,
                            `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                            '   </button>'
                        ].join('');
                    }
                    else {
                        return [
                            `   <button type="button" class="btn btn-sm bg-secondary edit-appointment" data-appointment-id="${row.id}" data-toggle="modal" data-target="#AppointmentEditModal">`,
                            `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                            '   </button>',
                        ].join('');
                    }
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
        var appointment = _$form.serializeFormToObject();
        if (abp.auth.grantedPermissions['Pages.Master.Organizations.Dropdown'] == true)//Super admin or Admin 
        {
            appointment.organizationId = $('.organization-dropdown').children(":selected").attr("id");
            appointment.locationId = $('.location-dropdown').children(":selected").attr("value");
        }
        else if (_userLogSession.organizationId) {
            appointment.organizationId = _userLogSession.organizationId;
            appointment.locationId = _userLogSession.locationId;
        }
        else {
            abp.message.error(abp.utils.formatString(l('Please select Organization before create from dropdown in header.')));
        }
        appointment.assetId = $('.asset_dd').children(":selected").attr("value");
        appointment.userId = _userLogSession.id;

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
        if (abp.auth.grantedPermissions['Pages.Master.Organizations.Dropdown'] == true && $('.organization-dropdown')[0].value != "All Organization") {
            $("#AppointmentCreateModal").addClass('show');
            $("#AppointmentCreateModal").show();
            $('.nav-tabs a[href="#appointment-details"]').tab('show');
        }
        else if (_userLogSession.organizationId != defaultGuid) {
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

    if (abp.auth.grantedPermissions['Pages.Master.Organizations.Dropdown'] == true) {
        document.getElementById('org_dropdown').addEventListener("change", function (e) {
            let org_ = document.getElementById('org_dropdown');
            let organization_id = org_.options[org_.selectedIndex].id;

            e.preventDefault();
            abp.ajax({
                url: abp.appPath + 'Appointments/FillLocation?orgId=' + organization_id,
                type: 'GET',
                dataType: 'JSON',
                success: function (locations) {
                    $("#loc_dropdown").html("");
                    $.each(locations.Result, function (i, loc) {
                        $element1 = $('<option></option>').val(loc.Id).html(loc.Address1 + ' ' + loc.Address2);

                        if (i === 0) {
                            $element1 = $element1.attr("selected", "selected");
                        }
                        $("#loc_dropdown").append($element1);
                    });
                    abp.ajax({
                        url: abp.appPath + 'Appointments/FillAsset?locationId=' + $("#loc_dropdown")[0].value,
                        type: 'GET',
                        dataType: 'JSON',
                        success: function (assets) {
                            debugger;
                            if (assets.Result.length == 0) {
                                $("#asset_data").html("");
                                $element1 = $('<option></option>').html("Select asset").attr("disabled", "disabled").attr("selected", "selected");
                                $("#asset_data").append($element1);
                            } else {
                                $("#asset_data").html(""); // clear before appending new list 
                                $.each(assets.Result, function (i, asset) {
                                    $element1 = $('<option></option>').val(asset.Id).html(asset.Name);

                                    if (i === 0) {
                                        $element1 = $element1.attr("selected", "selected");
                                    }
                                    $("#asset_data").append($element1);
                                });
                            }
                        },
                        error: function (e) {
                        }
                    });

                },
                error: function (e) {
                }
            });
        });

        document.getElementById('loc_dropdown').addEventListener("change", function (e) {
            if (!$("#loc_dropdown option")[0].selected) {
                $("#loc_dropdown option")[0].removeAttribute("selected", "selected");
                $("#loc_dropdown option:selected")[0].setAttribute("selected", "selected");
            }

            let location_ = document.getElementById('loc_dropdown');
            let location_id = location_.options[location_.selectedIndex].value;

            e.preventDefault();
            abp.ajax({
                url: abp.appPath + 'Appointments/FillAsset?locationId=' + location_id,
                type: 'GET',
                dataType: 'JSON',
                success: function (assets) {
                    debugger;
                    if (assets.Result.length == 0) {
                        $("#asset_data").html("");
                        $element1 = $('<option></option>').html("Select asset").attr("disabled", "disabled").attr("selected", "selected");
                        $("#asset_data").append($element1);
                    } else {
                        $("#asset_data").html(""); // clear before appending new list 
                        $.each(assets.Result, function (i, asset) {
                            $element1 = $('<option></option>').val(asset.Id).html(asset.Name);

                            if (i === 0) {
                                $element1 = $element1.attr("selected", "selected");
                            }
                            $("#asset_data").append($element1);
                        });
                    }
                },
                error: function (e) {
                }
            });
        });

        document.getElementById('asset_data').addEventListener("change", function () {
            if (!$("#asset_data option")[0].selected) {
                $("#asset_data option")[0].removeAttribute("selected", "selected");
                $("#asset_data option:selected")[0].setAttribute("selected", "selected");
            }
        });
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