var TieneSucursal = true;

$(function () {
    loadTable();

        bsCustomFileInput.init();
       $("#cargando").hide()
});

$("#frmupload").submit(function (event) {
   
    event.preventDefault();
    $("#subir").prop("disabled", true);
    $("#cargando").show()
    $("#subir").val("")
    var fileExtension = ['xls', 'xlsx'];

    var filename = $('#excel').val();

    if (filename.length == 0) {

        alertify.alert("Informacion", "Seleccione un archivo")
        $("#subir").prop("disabled", false);
        $("#cargando").hide()
        $("#subir").val("Cargar")
        return false;
    }
            else {
        var extension = filename.replace(/^.*\./, '');

            if ($.inArray(extension, fileExtension) == -1) {
                alertify.alert("Informacion", "solo se admiten archivos de excel")
                $("#subir").prop("disabled", false);
                $("#cargando").hide()
                $("#subir").val("Cargar")
            return false;
        }
        var fdata = new FormData();
        var fileUpload = $("#excel").get(0);
        var files = fileUpload.files;
        fdata.append(files[0].name, files[0]);
        $.ajax({
            url: urlUpload, // Url
            data: fdata,
            type: "post",  // Verbo HTTP
            processData: false,
            contentType: false,
        })
            // Se ejecuta si todo fue bien.
            .done(function (result) {
                if (result != null) {
                    if (result == "1") {
                        alertify.alert("Informacion", "Los registros fueron agregados!");
                        loadTable();
                        $("#subir").prop("disabled", false);
                        $("#cargando").hide()
                        $("#subir").val("Cargar")
                    } else {
                        alertify.alert("Informacion", result);
                        $("#subir").prop("disabled", false);
                        $("#cargando").hide()
                        $("#subir").val("Cargar")
                    }

                }
            })

            .fail(function (xhr, status, error) {

            })

            .always(function () {

            });



    }


});


function modalModificar(IdCliente) {
    $("#modalCRUD").modal('show');
    $("#idCliente").val(IdCliente);

    $.ajax({
        url: urlGetCliente, // Url
        data: { IdCliente: IdCliente      },
        type: "post"  // Verbo HTTP
    })
        // Se ejecuta si todo fue bien.
        .done(function (result) {
            if (result != null) {
                var outObjA = JSON.parse(JSON.stringify(result));
              

                $("#Codigo").val(outObjA.codigo)
                $("#NombreCliente").val(outObjA.nombreCliente)
                $("#NombreComercial").val(outObjA.nombreComercial)
                $("#Direccion").val(outObjA.direccion)
                $("#Cargo").val(outObjA.cargo)
                $("#Telefono").val(outObjA.telefono)
                $("#Email").val(outObjA.email)
                $("#Contacto").val(outObjA.contacto)
                $("#Movil").val(outObjA.movil)
               // $('#idDepartamento option[value=\'' + outObjA.idDepartamento + '\'').prop("selected", true);

                $('#idDepartamento').val( outObjA.keyDepartamento);
                $('#idDepartamento').trigger('change');
                setTimeout(function () {

                    $('#idMunicipio').val(outObjA.keyMunicipio);
                    $('#idMunicipio').trigger('change');
                }, 1000);  
    
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
    $("#Codigo").val("")
    $("#NombreCliente").val("")
    $("#NombreComercial").val("")
    $("#Direccion").val("")
    $("#Cargo").val("")
    $("#Telefono").val("")
    $("#Email").val("")
    $("#Contacto").val("")
    $("#Movil").val("")
    $("#idCliente").val("")

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
            { "data": "codigo" },
            { "data": "nombreCliente" },
            { "data": "nombreComercial" },
            { "data": "descripcionDep" },
            { "data": "descripcionMun" },
            { "data": "direccion" },
            {"data": null,
                "render": function (data, type, full, meta) {
                    return " <button class='btn btn-info btn-sm' onclick =modalModificar('" + full.idCliente+"');> <i class='material-icons'>Modificar</i></button >";
                }
                }
        ],
        "destroy": true, "scrollX": "200%",
        "scrollY": "350px",
        "lengthMenu": [[10, 20, 50, 100, 1000, -1], [10, 20, 50, 100, 1000, "All"]],
        "responsive": false, "lengthChange": true, "autoWidth": true, "searching": true
        //"buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"]
    }).buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');

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
    loadDepartamento();
    getMunicipio(1, 0);
   
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
                $("#Contacto").inputmask("9999-9999");
                $("#Telefono").inputmask("9999-9999");
                $("#Movil").inputmask("9999-9999");

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
    $("#idCliente").val(idSucursal)
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
    if ($("#idCliente").val() == "") {
        // Deshabilitamos el botón de Submit
        $("#btnGuardar").prop("disabled", true);
        //var data = $("#AjaxForm").serialize()
        $.ajax({
            url: urlInsert, // Url
            data: {
                IdEmpresa:idEmpresa,IdSucursal:idSucursal,Codigo: $("#Codigo").val(), NombreCliente: $("#NombreCliente").val(),
                NombreComercial: $("#NombreComercial").val(), Contacto: $("#Contacto").val(), Cargo: $("#Cargo").val(), Email: $("#Email").val()
                , Telefono: $("#Telefono").val(), Movil: $("#Movil").val(), Direccion: $("#Direccion").val(), KeyDepartamento: $("#idDepartamento").val(),
                KeyMunicipio: $("#idMunicipio").val()
            },
            type: "post"  // Verbo HTTP
        })
            // Se ejecuta si todo fue bien.
            .done(function (result) {
                if (result != null) {
                    //    console.log(result)
                    if (result == "1") {
                        alertify.alert("Informacion", "Operacion exitosa")
                        $("#Codigo").val("")
                        $("#NombreCliente").val("")
                        $("#NombreComercial").val("")
                        $("#Direccion").val("")
                        $("#Cargo").val("")
                        $("#Telefono").val("")
                        $("#Email").val("")
                        $("#Contacto").val("")
                        $("#Movil").val("")
                        loadTable();
                    } else {
                        $("#showfrm").html(result)
                        loadDepartamento();
                        getMunicipio(1, 0);
                        $("#Contacto").inputmask("9999-9999");
                        $("#Telefono").inputmask("9999-9999");
                        $("#Movil").inputmask("9999-9999");
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
            data: {
                idCliente:$("#idCliente").val(),IdEmpresa: idEmpresa, IdSucursal: idSucursal, Codigo: $("#Codigo").val(), NombreCliente: $("#NombreCliente").val(),
                NombreComercial: $("#NombreComercial").val(), Contacto: $("#Contacto").val(), Cargo: $("#Cargo").val(), Email: $("#Email").val()
                , Telefono: $("#Telefono").val(), Movil: $("#Movil").val(), Direccion: $("#Direccion").val(), KeyDepartamento: $("#idDepartamento").val(),
                KeyMunicipio: $("#idMunicipio").val()
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
                        loadDepartamento();
                        getMunicipio(1, 0);
                        $("#Contacto").inputmask("9999-9999");
                        $("#Telefono").inputmask("9999-9999");
                        $("#Movil").inputmask("9999-9999");
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


