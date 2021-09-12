$(document).ready(function () {
    LoadTable();
});
function LoadTable() {
    $('#invoicetable').DataTable({
        stateSave: true,
        "pagingType": "full_numbers",
        "lengthMenu": [[5, 10, 20, 50, -1], [5, 10, 20, 50, "All"]],
        columnDefs: [{
            "targets": 0,
            "orderable": false
        }],
        order: [[1, 'asc']],
    });
}
function PayInvoice(ele) {
    $.ajax({
        type: 'POST',
        url: 'Default.aspx/PayInvoice',
        data: JSON.stringify({ invoice_id: $(ele).attr('data-value') }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (response) {
            $("#ContentPlaceHolder1_dataSection").html(response.d);
            LoadTable();
        },
        error: function (err) {
        },
    });
}
function Details(ele) {
    $.ajax({
        type: 'POST',
        url: 'Default.aspx/Details',
        data: JSON.stringify({ invoice_id: $(ele).attr('data-value') }),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (response) {
            var tr = $(ele).closest('tr');
            var row = $("#invoicetable").DataTable().row(tr);

            if (row.child.isShown()) {
                //Hide Details
                row.child.hide();
                tr.removeClass('shown');
            }
            else {
                //Show Details
                row.child(response.d).show();
                tr.addClass('shown');
            }
        },
        error: function (err) {
        },
    });
}