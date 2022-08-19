(function ($) {
    var _locationService = abp.services.app.location,
        l = abp.localization.getSource('TechEngineer'),
        _$modal = $('#LocationCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#LocationsTable');

    var _$locationsTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _locationService.getAll,
            inputFilter: function () {
                return $('#LocationSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$locationsTable.draw(false)
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
                data: 'address1',
                sortable: false
            },
            {
                targets: 2,
                data: 'branchITHeadEmail',
                sortable: false
            },
            {
                targets: 3,
                data: 'landmark',
                sortable: false
            },
            {
                targets: 4,
                data: 'cityId',
                sortable: false
            },
            {
                targets: 5,
                data: 'stateId',
                sortable: false
            },
            {
                targets: 6,
                data: 'countryId',
                sortable: false
            },
            {
                targets: 7,
                data: 'organization.name',
                sortable: false
            },
            {
                targets: 8,
                data: 'isActive',
                sortable: false,
                render: data => `<input type="checkbox" disabled ${data ? 'checked' : ''}>`
            },
            {
                targets: 9,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    if (abp.auth.grantedPermissions['Pages.Locations.Delete'] == true) {
                        return [
                            `   <button type="button" class="btn btn-sm bg-secondary edit-location" data-location-id="${row.id}" data-toggle="modal" data-target="#LocationEditModal">`,
                            `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                            '   </button>',
                            `   <button type="button" class="btn btn-sm bg-danger delete-location" data-location-id="${row.id}" data-location-name="${row.name}">`,
                            `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                            '   </button>'
                        ].join('');
                    }
                    else if (abp.auth.grantedPermissions['Pages.Locations.Edit'] == true) {
                        return [
                            `   <button type="button" class="btn btn-sm bg-secondary edit-location" data-location-id="${row.id}" data-toggle="modal" data-target="#LocationEditModal">`,
                            `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                            '   </button>',
                        ].join('');
                    }
                    else {
                        return [
                            `<span></span>`
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
        var location = _$form.serializeFormToObject();
        location.OrganizationId = $('.organization-dropdown').children(":selected").attr("id");
        location.IsActive = true;

        abp.ui.setBusy(_$modal);
        _locationService.create(location).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            abp.notify.info(l('SavedSuccessfully'));
            $("#LocationCreateModal").hide();
            _$locationsTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });

    $(document).on('click', '.delete-location', function () {
        var locationId = $(this).attr("data-location-id");
        var locationName = $(this).attr('data-location-name');

        deleteLocation(locationId, locationName);
    });

    function deleteLocation(locationId, locationName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                locationName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _locationService.delete({
                        id: locationId
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$locationsTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-location', function (e) {
        var locationId = $(this).attr("data-location-id");
        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Locations/EditModal?locationId=' + locationId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#LocationEditModal div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    });

    $(document).on('click', 'a[id="LocationCreateBtn"]', (e) => {
        if ($('.organization-dropdown')[0].value != "All Organization") {
            $("#LocationCreateModal").addClass('show');
            $("#LocationCreateModal").show();
            $('.nav-tabs a[href="#location-details"]').tab('show');
        }
        else {
            $("#LocationCreateModal").removeClass('show');
            abp.message.error(abp.utils.formatString(l('Please select Organization before create from dropdown in header.')));
        }
    });

    $(document).on('click', '.close', (e) => {
        $("#LocationCreateModal").hide();
        _$form.clearForm();
    });

    _$form.find('.close-button').on('click', (e) => {
        $("#LocationCreateModal").hide();
        _$form.clearForm();
    });

    abp.event.on('location.edited', (data) => {
        _$locationsTable.ajax.reload();
    });

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        _$locationsTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$locationsTable.ajax.reload();
            return false;
        }
    });
})(jQuery);
