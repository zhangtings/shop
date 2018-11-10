layui.use(['form', 'layer', 'laydate', 'table', 'laytpl'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laydate = layui.laydate,
        laytpl = layui.laytpl,
        table = layui.table;

    //用户等级
    table.render({
        elem: '#roomType',
        url: '/Admin/Room/RTList',
        cellMinWidth: 95,
        cols: [[
            { field: "id", title: 'ID', width: 60, fixed: "left", sort: "true", align: 'center', edit: 'text' },
            { field: 'Name', title: '类型名称', templet: '#gradeIcon', align: 'center' },
            { field: 'Price', title: '价格', edit: 'text', align: 'center' },
            { field: 'AddPrice', title: '加床价格', edit: 'text', sort: "true", align: 'center' },
            { field: 'Mark', title: '备注', edit: 'text',  align: 'center' }
            //{ title: '当前状态', minWidth: 100, templet: '#rtBar', fixed: "right", align: "center" }
        ]]
    });

    form.on('switch(gradeStatus)', function (data) {
        var tipText = '确定禁用当前会员等级？';
        if (data.elem.checked) {
            tipText = '确定启用当前会员等级？'
        }
        layer.confirm(tipText, {
            icon: 3,
            title: '系统提示',
            cancel: function (index) {
                data.elem.checked = !data.elem.checked;
                form.render();
                layer.close(index);
            }
        }, function (index) {
            layer.close(index);
        }, function (index) {
            data.elem.checked = !data.elem.checked;
            form.render();
            layer.close(index);
        });
    });
    //新增等级
    $(".addGrade").click(function () {
        var $tr = $(".layui-table-body.layui-table-main tbody tr:last");
        if ($tr.data("index") < 9) {
            var newHtml = '<tr data-index="' + ($tr.data("index") + 1) + '">' +
                '<td data-field="id" data-edit="text" align="center"><div class="layui-table-cell laytable-cell-1-id">' + ($tr.data("index") + 2) + '</div></td>' +
                '<td data-field="gradeIcon" align="center" data-content="icon-vip' + ($tr.data("index") + 2) + '"><div class="layui-table-cell laytable-cell-1-gradeIcon"><span class="seraph vip' + ($tr.data("index") + 2) + ' icon-vip' + ($tr.data("index") + 2) + '"></span></div></td>' +
                '<td data-field="Name" data-edit="text" align="center"><div class="layui-table-cell laytable-cell-1-Name">类型名称</div></td>' +
                '<td data-field="Price" data-edit="text" align="center"><div class="layui-table-cell laytable-cell-1-Price">0</div></td>' +
                '<td data-field="AddPrice" data-edit="text" align="center"><div class="layui-table-cell laytable-cell-1-AddPrice">0</div></td>' +
                '<td data-field="Mark" data-edit="text" align="center"><div class="layui-table-cell laytable-cell-1-Mark">0</div></td>' +
                
                '</tr>';
            $(".layui-table-body.layui-table-main tbody").append(newHtml);
            $(".layui-table-fixed.layui-table-fixed-l tbody").append('<tr data-index="' + ($tr.data("index") + 1) + '"><td data-field="id" data-edit="text" align="center"><div class="layui-table-cell laytable-cell-1-id">' + ($tr.data("index") + 2) + '</div></td></tr>');
            $(".layui-table-fixed.layui-table-fixed-r tbody").append('<tr data-index="' + ($tr.data("index") + 1) + '"><td data-field="' + ($tr.data("index") + 1) + '" align="center" data-content="" data-minwidth="100"><div class="layui-table-cell laytable-cell-1-' + ($tr.data("index") + 1) + '"> <input type="checkbox" name="gradeStatus" lay-filter="gradeStatus" lay-skin="switch" lay-text="启用|禁用" checked=""><div class="layui-unselect layui-form-switch layui-form-onswitch" lay-skin="_switch"><em>启用</em><i></i></div></div></td></tr>');
            form.render();
        } else {
            layer.alert("目前之支持到vip10！", { maxWidth: 300 });
        }
    });

    //头工具栏事件
    table.on('toolbar(roomType)', function (obj) {
        var checkStatus = table.checkStatus(obj.config.id);
        switch (obj.event) {
            case 'newBtn':
                location.href = '/Admin/Room/RTEdit';
                //layer.alert(JSON.stringify(data));
                break;
            case 'editBtn':
                var cdata = checkStatus.data;
                location.href = '/Admin/Room/RTEdit?Id=' + cdata[0].Id;
                //layer.msg('选中了：' + data.length + ' 个');
                break;
            case 'delBtn':
                var cdata = checkStatus.data;
                location.href = '/Admin/Room/RTDelete?Id=' + cdata[0].Id;
                //layer.msg(checkStatus.isAll ? '全选' : '未全选');
                break;
        };
    });

    //控制表格编辑时文本的位置【跟随渲染时的位置】
    $("body").on("click", ".layui-table-body.layui-table-main tbody tr td", function () {
        $(this).find(".layui-table-edit").addClass("layui-" + $(this).attr("align"));
    });

})