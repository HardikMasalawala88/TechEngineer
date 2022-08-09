(function ($) {
    var _organizationService = abp.services.app.organization,
        l = abp.localization.getSource('TechEngineer'),
        _$modal = $('#OrganizationCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#OrganizationsTable');

    var _$organizationsTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _organizationService.getAll,
            inputFilter: function () {
                return $('#OrganizationSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$organizationsTable.draw(false)
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
                data: 'name',
                sortable: false
            },
            {
                targets: 2,
                data: 'primaryEmailAddress',
                sortable: false
            },
            {
                targets: 3,
                data: 'isActive',
                sortable: false,
                render: data => `<input type="checkbox" disabled ${data ? 'checked' : ''}>`
            },
            {
                targets: 4,
                data: null,
                sortable: false,
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-organization" data-organization-id="${row.id}" data-toggle="modal" data-target="#OrganizationEditModal">`,
                        `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-organization" data-organization-id="${row.id}" data-organization-name="${row.name}">`,
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

        var organization = _$form.serializeFormToObject();
        organization.location = {
            Address1: organization['AddressLine1'],
            Address2: organization['AddressLine2'],
            Landmark: organization['Landmark'],
            BranchITHeadEmail: organization['BranchITHeadEmail'],
            PostalCode: organization['PostalCode'],
            CityId: organization['City'],
            StateId: organization['State'],
            CountryId: organization['Country']
        };

        abp.ui.setBusy(_$modal);
        _organizationService.create(organization).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            abp.notify.info(l('SavedSuccessfully'));
            _$organizationsTable.ajax.reload();
            location.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });

    $(document).on('click', '.delete-organization', function () {
        var organizationId = $(this).attr("data-organization-id");
        var organizationName = $(this).attr('data-organization-name');

        deleteOrganization(organizationId, organizationName);
    });

    function deleteOrganization(organizationId, organizationName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                organizationName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _organizationService.delete({
                        id: organizationId
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$organizationsTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-organization', function (e) {
        var organizationId = $(this).attr("data-organization-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Organizations/EditModal?organizationId=' + organizationId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#OrganizationEditModal div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    });

    $(document).on('click', 'a[data-target="#OrganizationCreateModal"]', (e) => {
        $('.nav-tabs a[href="#organization-details"]').tab('show')
    });

    abp.event.on('organization.edited', (data) => {
        _$organizationsTable.ajax.reload();
    });

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        _$organizationsTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$organizationsTable.ajax.reload();
            return false;
        }
    });
})(jQuery);
