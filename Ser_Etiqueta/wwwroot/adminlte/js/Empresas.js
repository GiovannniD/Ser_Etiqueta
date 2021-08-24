var TieneSucursal = true;

$(function () {

    loadTable();
});
function modalModificar(IdEmpresa) {
    $("#modalCRUD").modal('show');
    $("#IdEmpresa").val(IdEmpresa);

    $.ajax({
        url: urlGetEmpresa, // Url
        data: { IdEmpresa: IdEmpresa      },
        type: "post"  // Verbo HTTP
    })
        // Se ejecuta si todo fue bien.
        .done(function (result) {
            if (result != null) {
                var outObjA = JSON.parse(JSON.stringify(result));
                $("#NombreComercial").val(outObjA.nombreComercial)
                $("#NombreEmpresa").val(outObjA.nombreEmpresa)
                $("#IdSersa").val(outObjA.idSersa)
                $("#consecutivo").val(outObjA.ultimoConsecutivo)
                $("#codigoEtiqueta").val(outObjA.serieCodigoEtiqueta)
               // $('#idDepartamento option[value=\'' + outObjA.idDepartamento + '\'').prop("selected", true);

                $('#idDepartamento').val( outObjA.idDepartamento);
                $('#idDepartamento').trigger('change');
                setTimeout(function () {

                    $('#idMunicipio').val(outObjA.idMunicipio);
                    $('#idMunicipio').trigger('change');
                }, 2000);  
              
               // $('#idMunicipio').select2("val", "5");
                TieneSucursal = outObjA.tieneSucursal;
                if (TieneSucursal == true) {
                    $("#tieneSucursal").prop('checked', true);
                } else {
                    $("#tieneSucursal").prop('checked', false);
                }
           
      

                
            }
        })
        // Se ejecuta si se produjo un error.
        .fail(function (xhr, status, error) {
         
        })
        .always(function () {

        });
}

function addNew() {
    $("#modalCRUD").modal('show');
    $("#NombreComercial").val("")
    $("#NombreEmpresa").val("")
    $("#IdSersa").val("")
    $("#consecutivo").val("")
    $("#codigoEtiqueta").val("")
    $("#IdEmpresa").val("")

}


function addNewSucursal() {
    $("#modalCRUDSucursal").modal('show');
   /* $("#NombreComercial").val("")
    $("#NombreEmpresa").val("")
    $("#IdSersa").val("")
    $("#consecutivo").val("")
    $("#codigoEtiqueta").val("")
    $("#IdEmpresa").val("")*/

}
function loadTable()
{
    $("#example").DataTable({
        "bDeferRender": true,
      
       

        "keys": {
            "clipboard": false
        },
        "ajax": {
            "url": urlLoad,
            "method": 'POST', //usamos el metodo POST
           "data": { buscar: $("#txt_buscar").val() }, //enviamos opcion 4 para que haga un SELECT
            "dataSrc": ""
        },
        "columns": [
            { "data": "idSersa" },
            { "data": "nombreComercial" },
            { "data": "descripcionDep" },
            { "data": "descripcionMun" },
            { "data": "fechaIngreso" },
            {"data": null,
                "render": function (data, type, full, meta) {
                    return " <button class='btn btn-info btn-sm' onclick =modalModificar('" + full.idEmpresa+"');> <i class='material-icons'>Modificar</i></button >";
                }
                }
        ],
        "destroy": true, "scrollX": "200%",
        "scrollY": "350px",
        "lengthMenu": [[10, 20, 50, 100, 1000, -1], [10, 20, 50, 100, 1000, "All"]],
        "responsive": false, "lengthChange": true, "autoWidth": true, "searching": true
        //"buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"]
    }).buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');



    $("#example2").DataTable({
        "bDeferRender": true,



        "keys": {
            "clipboard": false
        },
        "ajax": {
            "url": urlLoadSucursal,
            "method": 'POST', //usamos el metodo POST
            "data": { buscar: $("#txt_buscar").val() }, //enviamos opcion 4 para que haga un SELECT
            "dataSrc": ""
        },
        "columns": [
            { "data": "idSucursal" },
            { "data": "nombreSucursal" },
            { "data": "nombreEmpresa" },
            { "data": "descripcionDep" },
            { "data": "descripcionMun" },
            { "data": "fechaIngreso" },
            {
                "data": null,
                "render": function (data, type, full, meta) {
                    return " <button class='btn btn-info btn-sm' onclick =modalModificarSucursal('" + full.idSucursal + "');> <i class='material-icons'>Modificar</i></button >";
                }
            }
        ],
        "destroy": true, //"dom": 'Blfrtip',
        "lengthMenu": [[10, 20, 50, 100, 1000, -1], [10, 20, 50, 100, 1000, "All"]],
        "responsive": false, "lengthChange": true, "autoWidth": true, "searching": true,
      //  "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"]
    }).buttons().container().appendTo('#example2_wrapper .col-md-6:eq(0)');
}


$('#tieneSucursal').change(function () {
    if (this.checked) {
        TieneSucursal = true;
    } else {
        TieneSucursal = false;
       // alert(TieneSucursal)
    }
});

function Form() {
    $.ajax({
        url: urlForm, // Url
        data: "",
        type: "post"  // Verbo HTTP
    })
        // Se ejecuta si todo fue bien.
        .done(function (result) {
            if (result != null) {
             //   console.log(result)
                $("#showfrm").html(result)
                $("#btnGuardar").prop("disabled", false);
                
                // Mostramos un mensaje de éxito.
                //   $("#ExitoAlert").show("slow").delay(2000).hide("slow");
            }
        })
        // Se ejecuta si se produjo un error.
        .fail(function (xhr, status, error) {
            // Mostramos un mensaje de error.
            //    $("#ErrorAlert").show("slow").delay(2000).hide("slow");

            // Escondemos el Ajax Loader
            //  $("#AjaxLoader").hide("slow");

            // Habilitamos el botón de Submit
            //  $("#SubmitBtn").prop("disabled", false);
        })
        // Hacer algo siempre, haya sido exitosa o no.
        .always(function () {

        });
}

function modalModificarSucursal(idSucursal) {
    $("#idSucursal").val(idSucursal)
    $("#modalCRUDSucursal").modal("show")

    $.ajax({
        url: urlGetSucursal, // Url
        data: { IdSucursal: idSucursal },
        type: "post"  // Verbo HTTP
    })
        // Se ejecuta si todo fue bien.
        .done(function (result) {
            if (result != null) {
                var outObjA = JSON.parse(JSON.stringify(result));
                $("#nombreSucursal").val(outObjA.nombreSucursal)

                $('#idEmpresaSucursal').val(outObjA.idEmpresa);
                $('#idEmpresaSucursal').trigger('change');
                $('#idDepartamentoSucursal').val(outObjA.idDepartamento);
                $('#idDepartamentoSucursal').trigger('change');
                setTimeout(function () {

                    $('#idMunicipioSucursal').val(outObjA.idMunicipio);
                    $('#idMunicipioSucursal').trigger('change');
                }, 2000);

                // $('#idMunicipio').select2("val", "5");
                TieneSucursal = outObjA.tieneSucursal;
                if (TieneSucursal == true) {
                    $("#tieneSucursal").prop('checked', true);
                } else {
                    $("#tieneSucursal").prop('checked', false);
                }




            }
        })
        // Se ejecuta si se produjo un error.
        .fail(function (xhr, status, error) {

        })
        .always(function () {

        });
}

$("#btnGuardar").click(function () {
    // Mostramos el Ajax Loader
  //  $("#AjaxLoader").show("fast");
    if ($("#IdEmpresa").val() == "") {
        // Deshabilitamos el botón de Submit
        $("#btnGuardar").prop("disabled", true);
        //var data = $("#AjaxForm").serialize()
        $.ajax({
            url: urlInsert, // Url
            data: {
                IdSersa: $("#IdSersa").val(), NombreEmpresa: $("#NombreEmpresa").val(),
                NombreComercial: $("#NombreComercial").val(), TieneSucursal: TieneSucursal, IdDepartamento: $("#idDepartamento").val(),
                IdMunicipio: $("#idMunicipio").val(), SerieCodigoEtiqueta: $("#codigoEtiqueta").val(), UltimoConsecutivo: $("#consecutivo").val()
            },
            type: "post"  // Verbo HTTP
        })
            // Se ejecuta si todo fue bien.
            .done(function (result) {
                if (result != null) {
                    //    console.log(result)
                    if (result == "1") {
                        alertify.alert("Informacion", "Operacion exitosa")
                        $("#NombreComercial").val("")
                        $("#NombreEmpresa").val("")
                        $("#IdSersa").val("")
                        $("#consecutivo").val("")
                        $("#codigoEtiqueta").val("")
                        loadTable();
                        loadIdEmpresas()
                    } else {
                        $("#showfrm").html(result)
                    }

                    $("#btnGuardar").prop("disabled", false);
                    // loadDepartamento();
                    //   getMunicipio(1)

                    // Mostramos un mensaje de éxito.
                    //   $("#ExitoAlert").show("slow").delay(2000).hide("slow");
                }
            })
            // Se ejecuta si se produjo un error.
            .fail(function (xhr, status, error) {
                // Mostramos un mensaje de error.
                //    $("#ErrorAlert").show("slow").delay(2000).hide("slow");

                // Escondemos el Ajax Loader
                //  $("#AjaxLoader").hide("slow");

                // Habilitamos el botón de Submit
                //  $("#SubmitBtn").prop("disabled", false);
            })
            // Hacer algo siempre, haya sido exitosa o no.
            .always(function () {

            });

    } else {
        // Deshabilitamos el botón de Submit
        $("#btnGuardar").prop("disabled", true);
        //var data = $("#AjaxForm").serialize()
        $.ajax({
            url: urlUpdate, // Url
            data: { IdEmpresa:$("#IdEmpresa").val(),
                IdSucursal: $("#IdSersa").val(), NombreEmpresa: $("#NombreEmpresa").val(),
                NombreComercial: $("#NombreComercial").val(), TieneSucursal: TieneSucursal, IdDepartamento: $("#idDepartamento").val(),
                IdMunicipio: $("#idMunicipio").val(), SerieCodigoEtiqueta: $("#codigoEtiqueta").val(), UltimoConsecutivo: $("#consecutivo").val()
            },
            type: "post"  // Verbo HTTP
        })
            // Se ejecuta si todo fue bien.
            .done(function (result) {
                if (result != null) {
                   
                    //    console.log(result)
                    if (result == "1") {
                      //  $("#showfrm").html(result)
                        alertify.alert("Informacion", "Operacion exitosa")
                        loadTable();
                    } else {
                        $("#showfrm").html(result)
                    }

                    $("#btnGuardar").prop("disabled", false);
                    // loadDepartamento();
                    //   getMunicipio(1)

                    // Mostramos un mensaje de éxito.
                    //   $("#ExitoAlert").show("slow").delay(2000).hide("slow");
                }
            })
            // Se ejecuta si se produjo un error.
            .fail(function (xhr, status, error) {
                // Mostramos un mensaje de error.
                //    $("#ErrorAlert").show("slow").delay(2000).hide("slow");

                // Escondemos el Ajax Loader
                //  $("#AjaxLoader").hide("slow");

                // Habilitamos el botón de Submit
                //  $("#SubmitBtn").prop("disabled", false);
            })
            // Hacer algo siempre, haya sido exitosa o no.
            .always(function () {

            });

    }
});


$("#btnGuardarSucursal").click(function () {
    // Mostramos el Ajax Loader
    //  $("#AjaxLoader").show("fast");
    if ($("#idSucursal").val() == "") {
        // Deshabilitamos el botón de Submit
        $("#btnGuardarSucursal").prop("disabled", true);
        //var data = $("#AjaxForm").serialize()
        $.ajax({
            url: urlInsertSucursal, // Url
            data: {
                IdEmpresa: $("#idEmpresaSucursal").val(), NombreSucursal: $("#nombreSucursal").val(), IdDepartamento: $("#idDepartamentoSucursal").val(),
                IdMunicipio: $("#idMunicipioSucursal").val()
            },
            type: "post"  // Verbo HTTP
        })
            // Se ejecuta si todo fue bien.
            .done(function (result) {
                if (result != null) {
                    //    console.log(result)
                    if (result == "1") {
                        alertify.alert("Informacion", "Operacion exitosa")
                        $("#nombreSucursal").val("")
                        
                        loadTable();
                    } else {
                        alertify.alert("Informacion", "Ocurrio un error")
                    }

                    $("#btnGuardar").prop("disabled", false);
                    // loadDepartamento();
                    //   getMunicipio(1)

                    // Mostramos un mensaje de éxito.
                    //   $("#ExitoAlert").show("slow").delay(2000).hide("slow");
                }
            })
            // Se ejecuta si se produjo un error.
            .fail(function (xhr, status, error) {
                // Mostramos un mensaje de error.
                //    $("#ErrorAlert").show("slow").delay(2000).hide("slow");

                // Escondemos el Ajax Loader
                //  $("#AjaxLoader").hide("slow");

                // Habilitamos el botón de Submit
                //  $("#SubmitBtn").prop("disabled", false);
            })
            // Hacer algo siempre, haya sido exitosa o no.
            .always(function () {

            });

    } else {
        // Deshabilitamos el botón de Submit
        $("#btnGuardarSucursal").prop("disabled", true);
        //var data = $("#AjaxForm").serialize()
        $.ajax({
            url: urlUpdateSucursal, // Url
            data: {
                IdSucursal:$("#idSucursal").val(),IdEmpresa: $("#idEmpresaSucursal").val(), NombreSucursal: $("#nombreSucursal").val(), IdDepartamento: $("#idDepartamentoSucursal").val(),
                IdMunicipio: $("#idMunicipioSucursal").val()
            },
            type: "post"  // Verbo HTTP
        })
            // Se ejecuta si todo fue bien.
            .done(function (result) {
                if (result != null) {

                    //    console.log(result)
                    if (result == "1") {
                        //  $("#showfrm").html(result)
                        alertify.alert("Informacion", "Operacion exitosa")
                        loadTable();
                     //   $("#idSucursal").val("")
                    } else {
                        alertify.alert("Informacion", "Ocurrio un error")
                        //$("#showfrm").html(result)
                    }

                    $("#btnGuardarSucursal").prop("disabled", false);
                    // loadDepartamento();
                    //   getMunicipio(1)

                    // Mostramos un mensaje de éxito.
                    //   $("#ExitoAlert").show("slow").delay(2000).hide("slow");
                }
            })
            // Se ejecuta si se produjo un error.
            .fail(function (xhr, status, error) {
                // Mostramos un mensaje de error.
                //    $("#ErrorAlert").show("slow").delay(2000).hide("slow");

                // Escondemos el Ajax Loader
                //  $("#AjaxLoader").hide("slow");

                // Habilitamos el botón de Submit
                //  $("#SubmitBtn").prop("disabled", false);
            })
            // Hacer algo siempre, haya sido exitosa o no.
            .always(function () {

            });

    }
});