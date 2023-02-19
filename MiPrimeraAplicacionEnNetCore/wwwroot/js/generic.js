function mostrarModal(titulo = "¿Quieres agregar el nuevo registro?", texto = "Este registro se insertará a la base de datos.", button = "Agregar") {
    return Swal.fire({
        title: titulo,
        text: texto,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: button
    })
}


// Imprime reportes desde las vistas
function Imprimir() {
    var checkB = document.getElementById("checkB").outerHTML;
    var table = "<h2>Reporte Especialidad</h2>";
    table += document.getElementById("table").outerHTML;
    table = table.replace(checkB, "");
    var pagina = window.document.body;
    var ventana = window.open();
    ventana.document.write(table);
    ventana.print();
    ventana.close();
    window.document.body = pagina;
}

// Carga el paginado en las vistas
window.onload = function () {
    $(document).ready(function () {
        $('#table').DataTable();
    });
}

// Exportar vistas
function ExportarExcel() {
    document.getElementById("tipoReporte").value = "Excel";
    var frmReporte = document.getElementById("frmReporte");
    frmReporte.submit();
}

function ExportarWord() {
    document.getElementById("tipoReporte").value = "Word";
    var frmReporte = document.getElementById("frmReporte");
    frmReporte.submit();
}

function ExportarPDF(nombreController) {
    //document.getElementById("tipoReporte").value = "PDF";
    //var frmReporte = document.getElementById("frmReporte");
    //frmReporte.submit();

    var frm = new FormData();
    var checks = document.getElementsByName("nombrePropiedades");
    var nchecks = checks.length;

    for (var i = 0; i < nchecks; i++) {
        if (checks[i].checked == true) {
            frm.append("nombrePropiedades[]", checks[i].value);
        }
    }

    $.ajax({
        type: "POST",
        url: nombreController + "/exportarDatosPDF",
        data: frm,
        contentType: false,
        processData: false,
        success: function (data) {
            // Retorno base64
            var a = document.createElement("a");
            a.href = data;
            a.download = "reporte.pdf";
            a.click();
        }
    })

}
