(function ($) {
    var _assetService = abp.services.app.asset,
        l = abp.localization.getSource('TechEngineer'),
        _$modal = $('#AssetCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#AssetsTable');

    var _$assetsTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _assetService.getAll,
            inputFilter: function () {
                return $('#AssetsSearchForm').serializeFormToObject(true);
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
        abp.ui.setBusy(_$modal);
        _assetService.create(asset).done(function () {
            _$modal.modal('hide');
            _$form[0].reset();
            abp.notify.info(l('SavedSuccessfully'));
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
        if ($('.organization-dropdown')[0].value != "All Organization")
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

    //$(document).on('click', 'a[data-target="#AssetCreateModal"]', (e) => {
    //    debugger;
    //    $('.nav-tabs a[href="#asset-details"]').tab('show')
    //});

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