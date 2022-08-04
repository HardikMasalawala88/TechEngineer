(function ($) {
    var defaultGuid = "00000000-0000-0000-0000-000000000000";
    var _assetService = abp.services.app.asset,
        _userLogSession = ""
        l = abp.localization.getSource('TechEngineer'),
        _$modal = $('#AssetCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#AssetsTable');

    if (!_userLogSession) {
        abp.services.app.session.getCurrentLoginInformations().then(data => {
            _userLogSession = data.user;
        });
    }

    var _$assetsTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _assetService.getAll,
            inputFilter: function () {
                return $('#AssetSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$assetsTable.draw(false)
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
                data: 'category',
                sortable: false
            },
            {
                targets: 3,
                data: 'serialNumber',
                sortable: false
            },
            {
                targets: 4,
                data: 'modelNumber',
                sortable: false
            },
            {
                targets: 5,
                data: 'isActive',
                sortable: false,
                render: data => `<input type="checkbox" disabled ${data ? 'checked' : ''}>`
            },
            {
                targets: 6,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    if (abp.auth.grantedPermissions['Pages.Assets.Edit'] == true)
                    {
                        return [
                            `   <button type="button" class="btn btn-sm bg-secondary edit-asset" data-asset-id="${row.id}" data-toggle="modal" data-target="#AssetEditModal">`,
                            `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                            '   </button>',
                        ].join('');
                    }
                    else{
                        return [
                            `   <button type="button" class="btn btn-sm bg-secondary edit-asset" data-asset-id="${row.id}" data-toggle="modal" data-target="#AssetEditModal">`,
                            `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                            '   </button>',
                            `   <button type="button" class="btn btn-sm bg-danger delete-asset" data-asset-id="${row.id}" data-asset-name="${row.name}">`,
                            `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                            '   </button>'
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
        debugger;
        var asset = _$form.serializeFormToObject();
        if (abp.auth.grantedPermissions['Pages.Master.Organizations.Dropdown'] == true)//Super admin or Admin 
        {
            asset.organizationId = $('.organization-dropdown').children(":selected").attr("id");
            asset.locationId = $('.location_dd').children(":selected").attr("id");
        }
        else if (_userLogSession.organizationId) {
            asset.organizationId = _userLogSession.organizationId;
            asset.locationId = _userLogSession.locationId;
        }
        else {
            abp.message.error(abp.utils.formatString(l('Please select Organization before create from dropdown in header.')));
        }

        abp.ui.setBusy(_$modal);
        _assetService.create(asset).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            abp.notify.info(l('SavedSuccessfully'));
            $("#AssetCreateModal").hide()
            _$assetsTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });

    $(document).on('click', '.delete-asset', function () {
        var assetId = $(this).attr("data-asset-id");
        var assetName = $(this).attr('data-asset-name');

        deleteAsset(assetId, assetName);
    });

    function deleteAsset(assetId, assetName) {
        abp.message.confirm(
            abp.utils.formatString(
                l('AreYouSureWantToDelete'),
                assetName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _assetService.delete({
                        id: assetId
                    }).done(() => {
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$assetsTable.ajax.reload();
                    });
                }
            }
        );
    }

    $(document).on('click', '.edit-asset', function (e) {
        var assetId = $(this).attr("data-asset-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Assets/EditModal?assetId=' + assetId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#AssetEditModal div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    }); 

    $(document).on('click', 'a[id="CreateAssetsBtn"]', (e) => {
        debugger;
        if (abp.auth.grantedPermissions['Pages.Master.Organizations.Dropdown'] == true && $('.organization-dropdown')[0].value != "All Organization")
        {
            $("#AssetCreateModal").addClass('show');
            $("#AssetCreateModal").show();
            $('.nav-tabs a[href="#asset-details"]').tab('show');
            $("#location_dropdown")[0].value = $("#loc_dropdown")[0].value;
        }
        else if (_userLogSession.organizationId != defaultGuid)
        {          
            $("#AssetCreateModal").addClass('show');
            $("#AssetCreateModal").show();
            $('.nav-tabs a[href="#asset-details"]').tab('show');
        }
        else {
            $("#AssetCreateModal").removeClass('show');
            abp.message.error(abp.utils.formatString(l('Please select Organization before create from dropdown in header.')));
        }
    });

    $(document).on('click', '.close', (e) => {
        $("#AssetCreateModal").hide();
        _$form.clearForm();
    });

    _$form.find('.close-button').on('click', (e) => {
        $("#AssetCreateModal").hide();
        _$form.clearForm();
    });

    if (abp.auth.grantedPermissions['Pages.Master.Organizations.Dropdown'] == true) {
        document.getElementById('org_dropdown').addEventListener("change", function () {
            organization_selection();
        });
    }

    function organization_selection() {
        let org_ = document.getElementById('org_dropdown');
        let organization_id = org_.options[org_.selectedIndex].id;
        location_(organization_id);
    }

    function location_(organization_id) {
        let locations = document.getElementById('loc_dropdown');
        for (var i = 0; i < locations.options.length; i++)
        {
            if (locations.options[i].dataset.bind == organization_id) {
                $("#loc_dropdown")[0].value = locations.options[i].value;
            }
            else {
                //$("#loc_dropdown option[value=locations.options[i].value").remove();
                //locations.options[i].remove();
            }
        }

        let location_dd = document.getElementById('location_data');
        for (var i = 0; i < location_dd.options.length; i++) {
            if (location_dd.options[i].dataset.bind == organization_id) {
                $("#location_data")[0].value = $("#loc_dropdown")[0].value;
            }
            else {
                //$("#loc_dropdown")[0].remove(locations.options[i]);
            }
        }
    }

    abp.event.on('asset.edited', (data) => {
        _$assetsTable.ajax.reload();
    });

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        _$assetsTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$assetsTable.ajax.reload();
            return false;
        }
    });
})(jQuery);