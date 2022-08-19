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
                data: 'department',
                sortable: false
            },
            {
                targets: 3,
                data: 'category',
                sortable: false
            },
            {
                targets: 4,
                data: 'system_Username',
                sortable: false
            },
            {
                targets: 5,
                data: 'monitor',
                sortable: false
            },
            {
                targets: 6,
                data: 'monitor_SerialNo',
                sortable: false
            },
            {
                targets: 7,
                data: 'isActive',
                sortable: false,
                render: data => `<input type="checkbox" disabled ${data ? 'checked' : ''}>`
            },
            {
                targets: 8,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    if (abp.auth.grantedPermissions['Pages.Assets.Delete'] == true) {
                        return [
                            `   <button type="button" class="btn btn-sm bg-secondary edit-asset" data-asset-id="${row.id}" data-toggle="modal" data-target="#AssetEditModal">`,
                            `       <i class="fas fa-pencil-alt"></i> ${l('Edit')}`,
                            '   </button>',
                            `   <button type="button" class="btn btn-sm bg-danger delete-asset" data-asset-id="${row.id}" data-asset-name="${row.name}">`,
                            `       <i class="fas fa-trash"></i> ${l('Delete')}`,
                            '   </button>'
                        ].join('');
                    }
                    else {
                        return [
                            `   <button type="button" class="btn btn-sm bg-secondary edit-asset" data-asset-id="${row.id}" data-toggle="modal" data-target="#AssetEditModal">`,
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
        var asset = _$form.serializeFormToObject();
        if (abp.auth.grantedPermissions['Pages.Master.Organizations.Dropdown'] == true) //Super admin or Admin 
        {
            asset.organizationId = $('.organization-dropdown').children(":selected").attr("id");
            asset.locationId = $('.location_dd').children(":selected").attr("value");
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
        if (abp.auth.grantedPermissions['Pages.Master.Organizations.Dropdown'] == true && $('.organization-dropdown')[0].value != "All Organization") {
            $("#AssetCreateModal").addClass('show');
            $("#AssetCreateModal").show();
            $('.nav-tabs a[href="#asset-details"]').tab('show');
        }
        else if (_userLogSession.organizationId != defaultGuid) {
            $("#AssetCreateModal").addClass('show');
            $("#AssetCreateModal").show();
            $('.nav-tabs a[href="#asset-details"]').tab('show');
        }
        else {
            $("#AssetCreateModal").removeClass('show');
            abp.message.error(abp.utils.formatString(l('Please select Organization before create from dropdown in header.')));
        }
    });

    $(document).on('click', 'a[id="ImportExcelBtn"]', (e) => {
        if (abp.auth.grantedPermissions['Pages.Master.Organizations.Dropdown'] == true && $('.organization-dropdown')[0].value != "All Organization") {
            $("#ImportAssetExcelModal").addClass('show');
            $("#ImportAssetExcelModal").show();
        }
        else if (_userLogSession.organizationId != defaultGuid) {
            $("#ImportAssetExcelModal").addClass('show');
            $("#ImportAssetExcelModal").show();
        }
        else {
            $("#ImportAssetExcelModal").removeClass('show');
            abp.message.error(abp.utils.formatString(l('Please select Organization before import from dropdown in header.')));
        }
    });

    $(document).on('click', 'a[id="ImportAssetsBtn"]', (e) => {
        var files = $("#importFile").get(0).files;

        var formData = new FormData();
        formData.append('importFile', files[0]); 

        abp.ajax({
            url: abp.appPath + 'Assets/ImportAssetsData',
            data: formData,
            type: 'POST',
            contentType: false,
            processData: false,
            success: function (data) {
                if (data.Status === 1) {
                    alert(data.Message);
                } else {
                    alert("Failed to Import");
                }
            }  
        });
    });

    $(document).on('click', '.close', (e) => {
        $("#AssetCreateModal").hide();
        $("#ImportAssetExcelModal").hide();
        _$form.clearForm();
    });

    _$form.find('.close-button').on('click', (e) => {
        $("#AssetCreateModal").hide();
        _$form.clearForm();
    });

    if (abp.auth.grantedPermissions['Pages.Master.Organizations.Dropdown'] == true) {
        document.getElementById('org_dropdown').addEventListener("change", function (e) {
            let org_ = document.getElementById('org_dropdown');
            let organization_id = org_.options[org_.selectedIndex].id;

            e.preventDefault();
            abp.ajax({
                url: abp.appPath + 'Assets/FillLocation?orgId=' + organization_id,
                type: 'GET',
                dataType: 'JSON',
                success: function (locations) {
                    // clear before appending new list 
                    $("#loc_dropdown").html("");
                    $("#location_data").html("");
                    $("#location_dropdown").html("");

                    $.each(locations.Result, function (i, loc) {
                        $element1 = $('<option></option>').val(loc.Id).html(loc.Address1 + ' ' + loc.Address2);
                        $element2 = $('<option></option>').val(loc.Id).html(loc.Address1 + ' ' + loc.Address2);
                        $element3 = $('<option></option>').val(loc.Id).html(loc.Address1 + ' ' + loc.Address2);

                        if (i === 0) {
                            $element1 = $element1.attr("selected", "selected");
                            $element2 = $element2.attr("selected", "selected");
                            $element3 = $element3.attr("selected", "selected");
                        }
                        $("#loc_dropdown").append($element1);
                        $("#location_data").append($element2);
                        $("#location_dropdown").append($element3);
                    });
                },
                error: function (e) {
                    console.log(e.message);
                }
            });
        });

        document.getElementById('loc_dropdown').addEventListener("change", function () {
            if (!$("#loc_dropdown option")[0].selected) {
                $("#loc_dropdown option")[0].removeAttribute("selected", "selected");
                $("#loc_dropdown option:selected")[0].setAttribute("selected", "selected");
            }
        });

        document.getElementById('location_data').addEventListener("change", function () {
            if (!$("#location_data option")[0].selected) {
                $("#location_data option")[0].removeAttribute("selected", "selected");
                $("#location_data option:selected")[0].setAttribute("selected", "selected");
            }
        });

        document.getElementById('location_dropdown').addEventListener("change", function () {
            if (!$("#location_dropdown option")[0].selected) {
                $("#location_dropdown option")[0].removeAttribute("selected", "selected");
                $("#location_dropdown option:selected")[0].setAttribute("selected", "selected");
            }
        });
    }

    abp.event.on('asset.edited', (data) => {
        _$assetsTable.ajax.reload();
    });

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    if (abp.auth.grantedPermissions['Pages.Master.Organizations.Dropdown'] == true ||
        abp.auth.grantedPermissions['Pages.Search.Records'] == true) {
        $('.btn-search').on('click', (e) => {
            _$assetsTable.ajax.reload();
        });

        $('.txt-search').on('keypress', (e) => {
            if (e.which == 13) {
                _$assetsTable.ajax.reload();
                return false;
            }
        });
    }
})(jQuery);