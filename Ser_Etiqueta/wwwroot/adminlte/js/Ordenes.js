var TieneSucursal = true;



function importarOrdenes() {
    alertify.confirm('Confirmar Accion', 'Esta seguro?', function () {
        $.ajax({
            url: urlimport, // Url
            data: "",
            type: "post"  // Verbo HTTP
        })
            // Se ejecuta si todo fue bien.
            .done(function (result) {
                if (result != null) {
                    //    console.log(result)
                    if (result == "1") {
                        alertify.success("Registros importados!")

                        loadTable();
                    } else {


                    }



                }
            })
            // Se ejecuta si se produjo un error.
            .fail(function (xhr, status, error) {

            })
            // Hacer algo siempre, haya sido exitosa o no.
            .always(function () {

            });
    }
        , function () {
            //  alertify.error('Cancelado')


        });
}
$(function () {


  
    loadTable();
    loadTipoPaquete(1)
    alertify.defaults.theme.ok = "btn btn-primary";
    alertify.defaults.theme.cancel = "btn btn-danger";
        bsCustomFileInput.init();
    $("#cargando").hide()
    $("#cantidad").inputmask('integer', { min: 1, max: 1000000000 })
    //$("#Factura").inputmask('integer', { min: 1, max: 1000000000 })
    $("#peso").inputmask({
        alias: 'numeric',
        allowMinus: false,
        digits: 2,
        max: 999.99
    });
});

function loadTipoPaquete(id) {
   // $('#tipoPaquete').html("");

    $.ajax({
        url: urlTipoPaquetes, // Url
        data: "",
        type: "get"  // Verbo HTTP
    })
        // Se ejecuta si todo fue bien.
        .done(function (result) {
            console.log(result)
            if (result != null) {

                var outObjA = JSON.parse(JSON.stringify(result));
               
                for (var i = 0; i < outObjA.length; i++) {
                    var jsonData = outObjA[i];
                   
                    $('#tipoPaquete' + id).append($("<option />").val(jsonData.idTipoPaquete).text(jsonData.desTipoPaquete));
         
                    //   console.log(jsonData.descripcionDep)
                    //$('#cargo_2').append($("<option />").val(jsonData.cargo).text(jsonData.cargo));
                    // console.log(jsonData.id);
                }
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


function loadTags() {
    // $('#tipoPaquete').html("");
    $.ajax({
        url: urlClientes, // Url
        data: "",
        type: "post"  // Verbo HTTP
    })
        // Se ejecuta si todo fue bien.
        .done(function (result) {
            console.log(result)
            if (result != null) {

                var outObjA = JSON.parse(JSON.stringify(result));

                for (var i = 0; i < outObjA.length; i++) {
                    var jsonData = outObjA[i];

                   //$('#tipoPaquete' + id).append($("<option />").val(jsonData.idTipoPaquete).text(jsonData.desTipoPaquete));
                    Tags.push(jsonData.nombreCliente);
                    TagsComercial.push(jsonData.nombreComercial)
                    //   console.log(jsonData.descripcionDep)
                    //$('#cargo_2').append($("<option />").val(jsonData.cargo).text(jsonData.cargo));
                    // 
                }
                AutoComplete();
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
    alertify.confirm('Confirmar Accion', 'Esta seguro?', function () { 
        $.ajax({
            url: urlInsert, // Url
            data: {
                IdEmpresa: idEmpresa, idSucursal: idSucursal
            },
            type: "post"  // Verbo HTTP
        })
            // Se ejecuta si todo fue bien.
            .done(function (result) {
                if (result != null) {
                    //    console.log(result)
                    if (result == "1") {
                        alertify.alert("Informacion", "Operacion exitosa")

                        loadTable();
                    } else {


                    }



                }
            })
            // Se ejecuta si se produjo un error.
            .fail(function (xhr, status, error) {

            })
            // Hacer algo siempre, haya sido exitosa o no.
            .always(function () {

            });
    }
        , function () {
            alertify.error('Cancelado')


        });

}

function updateEstado(idOrdenTrabajo) {
    alertify.confirm('Confirmar Accion', 'Esta seguro que desea cerrar esta orden?', function () {
        $.ajax({
            url: urlUpdate, // Url
            data: {
                idOrdenTrabajo: idOrdenTrabajo, estado:2
            },
            type: "post"  // Verbo HTTP
        })
            // Se ejecuta si todo fue bien.
            .done(function (result) {
                if (result != null) {
                    //    console.log(result)
                    if (result == "1") {
                        alertify.alert("Informacion", "Operacion exitosa")

                        loadTable();
                    } else {


                    }



                }
            })
            // Se ejecuta si se produjo un error.
            .fail(function (xhr, status, error) {

            })
            // Hacer algo siempre, haya sido exitosa o no.
            .always(function () {

            });
    }
        , function () {
            alertify.error('Cancelado')


        });

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
           "data": { id: $("#txt_buscar").val() }, //enviamos opcion 4 para que haga un SELECT
            "dataSrc": ""
        },
        "columns": [
            { "data": "idOrdenTrabajo" },
            { "data": "nombreComercial" },
            { "data": "fechaCreacion" },
            { "data": "estadoDes" },
            {
                "data": null,
                "className":'dt-body-left',
                "render": function (data, type, full, meta) {
                    if (full.estado == 1) {
                        var opciones = " <div class='text-left'><div class='btn-group'><button class='btn  btn-sm' style='background: #014377; color: white' onclick =verRemision('" + full.idOrdenTrabajo + "');> <i class='material-icons'>Ver Remision</i></button ><button class='btn  btn-sm btn-warning' style=' color: white'  onclick =updateEstado('" + full.idOrdenTrabajo + "');> <i class='material-icons'>Cerrar Orden</i></button><button class='btn  btn-sm' style='background: #014377; color: white' onclick =verOrden('" + full.idOrdenTrabajo + "');> <i class='material-icons'>Ver Orden</i></button ><button type='submit' class='btn  btn-sm' style='background: #BB0423; color: white' onclick =eliminarOt('" + full.idOrdenTrabajo + "'); > <i class='material-icons'>Eliminar</i></button ></div></div>";
                    } else if (full.estado) {
                        var opciones = " <div class='text-left'><div class='btn-group'><button class='btn  btn-sm' style='background: #014377; color: white' onclick =verRemision('" + full.idOrdenTrabajo + "');> <i class='material-icons'>Ver Remision</i></button ><button class='btn  btn-sm' style='background: #014377; color: white' onclick =verOrden('" + full.idOrdenTrabajo + "');> <i class='material-icons'>Ver Orden</i></button ></div></div>";
                    }
                    return opciones
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
            "url": urlLoad,
            "method": 'POST', //usamos el metodo POST
            "data": { idOrdenTrabajo: idOrdenTrabajo }, //enviamos opcion 4 para que haga un SELECT
            "dataSrc": ""
        },
        "columns": [
            { "data": null,
            "render": function (data, type, full, meta) {
                return meta.row + 1;
                }
            },
            { "data": "nombreCliente" },
            { "data": "nombreComercial" },
            { "data": "factura" },
            { "data": "descripcionMun" },
            { "data": "direccion" },
            { "data": "desTipoPaquete" },
            { "data": "peso" },
            { "data": "serie" },
            {
                "data": null,
                "className": 'dt-body-left',
                "render": function (data, type, full, meta) {
                  
                    var opciones = " <div class='text-left'><div class='btn-group'><button type='submit' class='btn  btn-sm' style='background: #014377; color: white' onclick =ImprimirEnvio('" + full.idOtCodigo + "'); > <i class='material-icons'>Ver Etiqueta</i></button ><button class='btn  btn-sm' style='background: #BB0423; color: white' onclick =Eliminar('" + full.idOtdetalle + "');> <i class='material-icons'>Eliminar</i></button ></div></div>";
                    if (Estado > 1) {
                        opciones = " <div class='text-left'><div class='btn-group'><button type='submit' class='btn  btn-sm' style='background: #014377; color: white' onclick =ImprimirEnvio('" + full.idOtCodigo + "'); > <i class='material-icons'>Ver Etiqueta</i></button ></div></div>";
                    }  
                    return opciones
                }
            }
        ],
        "destroy": true, "scrollX": "200%",
        "scrollY": "350px",
        "lengthMenu": [[10, 20, 50, 100, 1000, -1], [10, 20, 50, 100, 1000, "All"]],
        "responsive": true, "lengthChange": false, "autoWidth": true, "searching": true
        //"buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"]
    }).buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');


  
}

function filtro() {
    $("#example").DataTable({
        "bDeferRender": true,



        "keys": {
            "clipboard": false
        },
        "ajax": {
            "url": urlFiltro,
            "method": 'POST', //usamos el metodo POST
            "data": { fechaInicio: $("#DateInicio").val(), fechaFinal: $("#DateFinal").val() }, //enviamos opcion 4 para que haga un SELECT
            "dataSrc": ""
        },
        "columns": [
            { "data": "idOrdenTrabajo" },
            { "data": "nombreComercial" },
            { "data": "fechaCreacion" },
            { "data": "estadoDes" },
            {
                "data": null,
                "className": 'dt-body-left',
                "render": function (data, type, full, meta) {
                    if (full.estado == 1) {
                        var opciones = " <div class='text-left'><div class='btn-group'><button class='btn  btn-sm' style='background: #014377; color: white' onclick =verRemision('" + full.idOrdenTrabajo + "');> <i class='material-icons'>Ver Remision</i></button ><button class='btn  btn-sm' style='background: #014377; color: white' onclick =verOrden('" + full.idOrdenTrabajo + "');> <i class='material-icons'>Ver Orden</i></button ><button type='submit' class='btn  btn-sm' style='background: #BB0423; color: white' onclick =eliminarOt('" + full.idOrdenTrabajo + "'); > <i class='material-icons'>Eliminar</i></button ></div></div>";
                    } else if (full.estado) {
                        var opciones = " <div class='text-left'><div class='btn-group'><button class='btn  btn-sm' style='background: #014377; color: white' onclick =verRemision('" + full.idOrdenTrabajo + "');> <i class='material-icons'>Ver Remision</i></button ><button class='btn  btn-sm' style='background: #014377; color: white' onclick =verOrden('" + full.idOrdenTrabajo + "');> <i class='material-icons'>Ver Orden</i></button ></div></div>";
                    }
                    return opciones
                }
            }
        ],
        "destroy": true, "scrollX": "200%",
        "scrollY": "350px",
        "lengthMenu": [[10, 20, 50, 100, 1000, -1], [10, 20, 50, 100, 1000, "All"]],
        "responsive": false, "lengthChange": true, "autoWidth": true, "searching": true
        //"buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"]
    }).buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');


    $('#example3 tfoot th').each(function () {
        var title = $('#example thead th').eq($(this).index()).text();
        $(this).html('<input type="text" placeholder="Buscar..' + title + '" class="form-control"/>');
    });
    $("#example3").DataTable({
        "bDeferRender": true,



        "keys": {
            "clipboard": false
        },
        "ajax": {
            "url": urlEnvios,
            "method": 'POST', //usamos el metodo POST
            "data": { fecha1: $("#DateInicio").val(), fecha2: $("#DateFinal").val() }, //enviamos opcion 4 para que haga un SELECT
            "dataSrc": ""
        }, initComplete: function () {
            // Apply the search
            this.api().columns().every(function () {
                var that = this;

                $('input', this.footer()).on('keyup change clear', function () {
                    if (that.search() !== this.value) {
                        that
                            .search(this.value)
                            .draw();
                    }
                });
            });
        },
        "columns": [
            { "data": "n_registro" },
            { "data": "codigo" },
            { "data": "factura" },
            { "data": "paquete" },
            { "data": "codigoCliente" },
            { "data": "nombreComercial" },
            { "data": "direccion" },
            { "data": "contacto" },
            { "data": "municipio" },
            { "data": "peso" },
            { "data": "cantidadBulto" },
            { "data": "fechaCreacion" }

        ],
        "destroy": true, "scrollX": "200%", "dom": 'Blfrtip', "select": true,
        "scrollY": "350px",
        "lengthMenu": [[10, 20, 50, 100, 1000, -1], [10, 20, 50, 100, 1000, "All"]],
        "responsive": false, "lengthChange": true, "autoWidth": true, "searching": true,
        "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"]
    }).buttons().container().appendTo('#example1_wrapper .col-md-6:eq(0)');
}


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

function verOrden(id) {
    window.location.href = '/Orden/OrdenDetalle/' + id;
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


function cliente(Codigo) {
    $.ajax({
        url: urlGetCliente, // Url
        data: {
            Codigo: Codigo
        },
        type: "post"  // Verbo HTTP
    })
        // Se ejecuta si todo fue bien.
        .done(function (result) {
            if (result != null) {
                var outObjA = JSON.parse(JSON.stringify(result));
                $("#idCliente").val(outObjA[0].idCliente)
                $("#nombreCliente").val(outObjA[0].nombreCliente)
                $("#nombreComercial").val(outObjA[0].nombreComercial)
                $('#idMunicipio').val(outObjA[0].keyMunicipio);
                $('#idMunicipio').trigger('change');
                $("#direccion").val(outObjA[0].direccion)
              //  console.log(outObjA.direccion)

            }
        })
        // Se ejecuta si se produjo un error.
        .fail(function (xhr, status, error) {

        })
        // Hacer algo siempre, haya sido exitosa o no.
        .always(function () {

        });
}

function clienteNombre(Nombre) {
    $.ajax({
        url: urlGetClienteNombre, // Url
        data: {
            Nombre: Nombre
        },
        type: "post"  // Verbo HTTP
    })
        // Se ejecuta si todo fue bien.
        .done(function (result) {
            if (result != null) {
                var outObjA = JSON.parse(JSON.stringify(result));
                $("#Codigo").val(outObjA[0].codigo)
                $("#idCliente").val(outObjA[0].idCliente)
                $("#nombreCliente").val(outObjA[0].nombreCliente)
                $("#nombreComercial").val(outObjA[0].nombreComercial)
                $('#idMunicipio').val(outObjA[0].keyMunicipio);
                $('#idMunicipio').trigger('change');
                $("#direccion").val(outObjA[0].direccion)
                //  console.log(outObjA.direccion)

            }
        })
        // Se ejecuta si se produjo un error.
        .fail(function (xhr, status, error) {

        })
        // Hacer algo siempre, haya sido exitosa o no.
        .always(function () {

        });
}

function clienteNombreComercial(Nombre) {
    $.ajax({
        url: urlGetClienteNombreComercial, // Url
        data: {
            Nombre: Nombre
        },
        type: "post"  // Verbo HTTP
    })
        // Se ejecuta si todo fue bien.
        .done(function (result) {
            if (result != null) {
                var outObjA = JSON.parse(JSON.stringify(result));
                $("#Codigo").val(outObjA[0].codigo)
                $("#idCliente").val(outObjA[0].idCliente)
                $("#nombreCliente").val(outObjA[0].nombreCliente)
                $("#nombreComercial").val(outObjA[0].nombreComercial)
                $('#idMunicipio').val(outObjA[0].keyMunicipio);
                $('#idMunicipio').trigger('change');
                $("#direccion").val(outObjA[0].direccion)
                //  console.log(outObjA.direccion)

            }
        })
        // Se ejecuta si se produjo un error.
        .fail(function (xhr, status, error) {

        })
        // Hacer algo siempre, haya sido exitosa o no.
        .always(function () {

        });
}
$("#peso1").inputmask({
    alias: 'numeric',
    allowMinus: false,
    digits: 2,
    max: 999.99
});
var input = document.getElementById("Codigo");
input.addEventListener("keyup", function (event) {
    // Number 13 is the "Enter" key on the keyboard
    if (event.keyCode === 13) {
        // Cancel the default action, if needed
        event.preventDefault();

        if ($("#Codigo").val() != "") {
            //  descuento=parseFloat($("#descuento").val()/100)
            //  descuento = parseFloat($("#total").val().replace("$", "")) * parseFloat($("#descuento").val() / 100)
            //  alertify.alert($("#Codigo").val())
            cliente($("#Codigo").val());
        } else {
            // descuento = 0;
        }
        //   calcularMontos();
        // Trigger the button element with a click
        // document.getElementById("myBtn").click();
        //console.log(descuento)
        // $("#total").val(parseFloat($("#total").val().replace("$","")-descuento))
        //    $('#descuento').prop('readonly', true);
    }
});


var input2 = document.getElementById("nombreCliente");
input2.addEventListener("keyup", function (event) {
    // Number 13 is the "Enter" key on the keyboard
    if (event.keyCode === 13) {
        // Cancel the default action, if needed
        event.preventDefault();

        if ($("#nombreCliente").val() != "") {
            //  descuento=parseFloat($("#descuento").val()/100)
            //  descuento = parseFloat($("#total").val().replace("$", "")) * parseFloat($("#descuento").val() / 100)
            //  alertify.alert($("#Codigo").val())
          //  clienteNombre($("#nombreCliente").val());
        } else {
            // descuento = 0;
        }
        //   calcularMontos();
        // Trigger the button element with a click
        // document.getElementById("myBtn").click();
        //console.log(descuento)
        // $("#total").val(parseFloat($("#total").val().replace("$","")-descuento))
        //    $('#descuento').prop('readonly', true);
    }
});


function AutoComplete() {
    $("#nombreCliente").autocomplete({
        source: Tags,
            select: function(event, ui) {
              //  alert();
                clienteNombre(ui.item.label)
            }
    });

    $("#nombreComercial").autocomplete({
        source: TagsComercial,
        select: function (event, ui) {
            //  alert();
            clienteNombreComercial(ui.item.label)
        }
    });
}
function addNewEnvio() {
    $("#agregar").prop("disabled", true);
    var peso = "";
    var tipo = "";
    var cantidad = $("#cantidad").val()
    if ($("#idCliente").val() == "" || $("#idCliente").val() == 0) {
        alertify.alert("informacion", "seleccione un cliente")
        $("#agregar").prop("disabled", false);
        return false;
    }
    if ($("#Codigo").val() == "" || $("#Factura").val() == "" || $("#cantidad").val() == ""
    ) {
        alertify.alert("informacion", "Rellene todos los campos obligatorios (*)")
        $("#agregar").prop("disabled", false);
        return false;
    }
    if ($("#peso1").val() == "") {
        $("#peso1").val("0")
    }
    //for (var i = 0; i < cantidad; i++) {
       // peso += $("#peso" + Math.floor(i + 1)).val() + ",";
      //  tipo += $("#tipoPaquete" + Math.floor(i + 1)).val() + ",";

        //este metodo hace obligatorio los campos de peso y tipo de paquete
       /* if ($("#peso" + Math.floor(i + 1)).val() == "" || $("#peso" + Math.floor(i + 1)).val() == 0) {
            alertify.alert("informacion", "Rellene todos los campos obligatorios (*)")
            $("#agregar").prop("disabled", false);
            return false;
        } else {
            peso += $("#peso" + Math.floor(i + 1)).val() + ",";
            tipo += $("#tipoPaquete" + Math.floor(i + 1)).val() + ",";
        }*/
    //}
    

    peso = $("#peso1").val();
    tipo = $("#tipoPaquete1").val();
    var cadena = $("#keyDestino").val();
    var arrayDeCadenas = cadena.split("-");

   
    $.ajax({
        url: urlInsert, // Url
        data: {
            IdOrdenTrabajo: idOrdenTrabajo, IdCliente: $("#idCliente").val(), Codigo: $("#Codigo").val(), Factura: $("#Factura").val(),idMunicipio:arrayDeCadenas[1],
            keyOrigen: $("#keyOrigen").val(), keyDestino: arrayDeCadenas[0], IdTipoPaquete: tipo, direccion: $.trim($("#direccion").val()),
            CantidadBulto: cantidad, Peso: peso,Destinatario:$("#Destinatario").val()
        },
        type: "post"  // Verbo HTTP 
    })
            // Se ejecuta si todo fue bien.
            .done(function (result) {
                if (result != null) {
                    if (result == "1") {
                        alertify.success("Envio agregado");
                        loadTable()
                        $("#agregar").prop("disabled", false);
                        $("#cantidad").val("0")
                        $("#detalle").html("")
                        //  $("#Factura").val("")
                    }
                }
            })
            // Se ejecuta si se produjo un error.
            .fail(function (xhr, status, error) {
                $("#agregar").prop("disabled", false);
                alertify.alert("informacion", "Ocurrio un error revise su conexion")
            })
            .always(function () {

            });
    
    
}
function ImprimirEnvio(id) {
    window.open('/Orden/imprimirEnvio/'+id, '_blank');
}
function imprimirAllEnvio() {
    window.open('/Orden/imprimirAllEnvio/' + idOrdenTrabajo, '_blank');
}
function verRemision(id) {
    window.open('/Remision/Remision/' + id, '_blank');
}

function descargarExcel() {
    const d = new Date($("#DateInicio").val())
    const d2 = new Date($("#DateFinal").val())
    console.log(d.toISOString().split('T')[0])
    window.open('/Orden/DownloadExcelRemision?fecha1=' + d.toISOString().split('T')[0] + '&fecha2=' + d2.toISOString().split('T')[0], '_blank');
}
function Eliminar(idDetalle)
{

    alertify.confirm('Confirmar Accion', 'Esta seguro?', function () {
        $.ajax({
            url: urlDelete, // Url
            data: {
                IdOtdetalle:idDetalle
            },
            type: "post"  // Verbo HTTP
        })
            // Se ejecuta si todo fue bien.
            .done(function (result) {
                if (result != null) {
                    //    console.log(result)
                    if (result == "1") {
                        alertify.success("Eliminado")

                        loadTable();
                    } else {


                    }



                }
            })
            // Se ejecuta si se produjo un error.
            .fail(function (xhr, status, error) {

            })
            // Hacer algo siempre, haya sido exitosa o no.
            .always(function () {

            });
    }
        , function () {
          //  alertify.error('Cancelado')


        });

}

function eliminarOt(idOt) {

    alertify.confirm('Confirmar Accion', 'Esta seguro?', function () {
        $.ajax({
            url: urlDeleteOt, // Url
            data: {
                IdOrdenTrabajo: idOt
            },
            type: "post"  // Verbo HTTP
        })
            // Se ejecuta si todo fue bien.
            .done(function (result) {
                if (result != null) {
                    //    console.log(result)
                    if (result == "1") {
                        alertify.success("Eliminado")

                        loadTable();
                    } else {


                    }



                }
            })
            // Se ejecuta si se produjo un error.
            .fail(function (xhr, status, error) {

            })
            // Hacer algo siempre, haya sido exitosa o no.
            .always(function () {

            });
    }
        , function () {
            //  alertify.error('Cancelado')


        });

}