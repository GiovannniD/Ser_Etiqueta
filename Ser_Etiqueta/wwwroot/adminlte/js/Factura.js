var count = 0;
var destino = [];
var paquete = [];
var Idpaquete = [];
var total = 0
var subTotal = 0
var iva = 0
var rowIndice = 0;
$(function () {

    getMunicipio();
    //loadCiente();
    loadTable()
    loadTipoPaquete(1)
    $("#precio").inputmask({ alias: "currency", prefix: 'C$ ' })
    $("#cantidad").inputmask('integer', { min: 1, max: 1000000000 })
    $("#total").inputmask({ alias: "currency", prefix: 'C$ ' })
    $("#subtotal").inputmask({ alias: "currency", prefix: 'C$ ' })
    $("#iva").inputmask({ alias: "currency", prefix: '' })
    LoadDetalle();

});

$("#agregar").click(function () {
    var tipoPaquete = $("#tipoPaquete").val();
    var arrayDeCadenas = idFactura.split("-");

    precio_producto = document.getElementById("precio").value;
    precio_producto = Math.abs(precio_producto.replace("C$", "").replace(".00", "").replace(",", ""))

    if ( $("#cantidad").val() == "" || $("#cantidad").val() == "0" || $("#precio").val() == ""
        || precio_producto == "0.00" || precio_producto < 1 || $("#cantidad").val() < 1 ) {
        alertify.error("Asegurese de llenar todos los campos correctamente.")
        return false;
    }
    
    $.ajax({
        url: urlInsertDetalle,
        type: 'POST',
        dataType: 'json',
        data: {
            KeyFactura: arrayDeCadenas[1], KeyTipoPaquete: $("#tipoPaquete").val(), Cantidad: $("#cantidad").val(),
            DescripcionDetalle: $("#Origen option:selected").text() + "-" + $("#Destino option:selected").text(),
            PrecioUnitario: precio_producto,
            KeyOrigen: $("#Origen").val(), KeyDestino: $("#Destino").val(), Destinatario: ""},
        success: function (data, textStatus, xhr) {
            var outObjA = JSON.parse(JSON.stringify(data));
           // count++;
            LoadDetalle();
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation');
            altertify.alert("Ocurrio un error")


        }, complete: function () {

        }
    });

})

$('#detalle tbody').off('click', 'td.details');
$('#detalle tbody').on('click', 'td.details', function () {
    var tr = $(this).closest('tr');
    var col = tr.find('td')
    var rowIndex = tr.index();
    //  alert(rowIndex);
    alertify.confirm('Confirmar Accion', 'Esta seguro?', function () {
        var arrayDeCadenas = idFactura.split("-");
        count--;
        if (count == 0) {
            subTotal = 0;
            iva = 0;
            total = 0
        }

        $.ajax({
            url: urlDeleteDetalle,
            type: 'POST',
            dataType: 'json',
            data: { keyFacturaDetalle: $("#keyFactura" + rowIndex).val(), KeyFactura: arrayDeCadenas[1] },
            success: function (data, textStatus, xhr) {
                var outObjA = JSON.parse(JSON.stringify(data));
               /* $("#subtotal").val(outObjA[0].subtotal)
                $("#total").val(outObjA[0].total)
                $("#iva").val(outObjA[0].iva)
                alertify.alert("Registro Eliminado")
                tr.remove();*/
                LoadDetalle();
            },
            error: function (xhr, textStatus, errorThrown) {
                console.log('Error in Operation');
              

            }, complete: function () {

            }
        }); 
   

     

       // calcularMontos();
        // alert($("#precio").val())
    }
        , function () {
           // alertify.error('Cancelado')


        });

});


$('#detalle tbody').off('click', 'td.IdCantidad');
$('#detalle tbody').on('click', 'td.IdCantidad', function () {
    var tr = $(this).closest('tr');
    var col = tr.find('td')
    var rowIndex = tr.index();
    rowIndice = rowIndex;


});

$('#detalle tbody').off('click', 'td.IdPrecio');
$('#detalle tbody').on('click', 'td.IdPrecio', function () {
    var tr = $(this).closest('tr');
    var col = tr.find('td')
    var rowIndex = tr.index();
    rowIndice = rowIndex;


});

$('#detalle tbody').off('click', 'td.IdPaquete');
$('#detalle tbody').on('click', 'td.IdPaquete', function () {
    var tr = $(this).closest('tr');
    var col = tr.find('td')
    var rowIndex = tr.index();
    rowIndice = rowIndex;


});


function calcular() {
    subtotal = 0;
    var table = document.getElementById("detalle");
    for (var i = 1; i < table.rows.length; i++) {
        if (table.rows[i].cells.length) {
            subtotal = subtotal + parseFloat((table.rows[i].cells[3].textContent.trim())) * parseFloat((table.rows[i].cells[4].textContent.trim().replace("$", "")));
            console.log(subtotal);
        }

    }
}

function GenerarOrden() {

    alertify.confirm('Confirmar Accion', 'Esta seguro que desea generar una orden de trabajo?', function () {
        var arrayDeCadenas = idFactura.split("-");
      //  console.log(arrayDeCadenas[1].replace('000', ''))
        $.ajax({
            url: urlInsert, // Url
            data: {
                IdEmpresa: idEmpresa, idSucursal: idSucursal, noFactura: arrayDeCadenas[1]
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
function LoadDetalle() {
    $("#row").html(" <tr>      </tr >")
    count = 0;
    subTotal = 0
    iva = 0
    tota=0
    var arrayDeCadenas = idFactura.split("-");
    $.ajax({
        url: urlLoadDetalle, // Url
        data: { KeyFactura: arrayDeCadenas[1]},
        type: "post"  // Verbo HTTP
    })
        // Se ejecuta si todo fue bien.
        .done(function (result) {
           // console.log(result)
            if (result != null) {
                var outObjA = JSON.parse(JSON.stringify(result));

                for (var i = 0; i < outObjA.length; i++) {
                    var jsonData = outObjA[i];
                    count++;
                    if (admin == "True") {
                        $("#row").append('<tr>  <td >' + count + '  <input type="hidden" id="keyFactura' + count + '" value="' + outObjA[i].keyFacturaDetalle + '"></td> <td class=IdPaquete><select class="form-control select2bs4" style="width: 100%;" id="paquete'+count+'" > </select ></td>  <td>' + destino[jsonData.keyOrigen - 1] + '</td><td>' + destino[jsonData.keyDestino - 1] + '</td><td class=IdCantidad><input type="text" class="form-control " placeholder="Cantidad" id="cantidad' + count + '" value="' + jsonData.cantidad + '"></td><td class=IdPrecio ><input type="text" class="form-control " placeholder="Precio" id="precio' + count + '" value="' + jsonData.precioUnitario + '"></td><td >' + 'C$' + Math.abs(jsonData.precioUnitario * jsonData.cantidad) + '</td><td class=details ><button type="button" class="btn btn-danger ">Eliminar  <i class="fas fa-cart-arrow-down" ></i></button></td> </tr>');
                    } else {
                        $("#row").append('<tr>  <td >' + count + '  <input type="hidden" id="keyFactura' + count + '" value="' + outObjA[i].keyFacturaDetalle + '"></td> <td class=IdPaquete><select class="form-control select2bs4" style="width: 100%;" id="paquete'+count+'"> </select ></td>  <td>' + destino[jsonData.keyOrigen - 1] + '</td><td>' + destino[jsonData.keyDestino - 1] + '</td><td class=IdCantidad ><input type="text" class="form-control " placeholder="Cantidad" id="cantidad' + count + '" value="' + jsonData.cantidad + '"></td><td class=IdPrecio><input type="text" class="form-control " placeholder="Precio" id="precio' + count + '" value="' + jsonData.precioUnitario + '"></td><td >' + 'C$' + Math.abs(jsonData.precioUnitario * jsonData.cantidad) + '</td><td></td> </tr>');
                    }
                    
                    loadUpdatePaquete(count)
                    var c = $("#cantidad" + count).val()
                    var input2 = document.getElementById("cantidad" + count);
                    $('#paquete' + count).val(jsonData.keyTipoPaquete);
                    $('#paquete' + count).trigger('change');
                    console.log(jsonData.keyTipoPaquete)
                    $('#paquete' + count).change(function () {

                        alertify.confirm('Confirmar Accion', 'Esta seguro?', function () {
                            updateDetalle(rowIndice);
                        }
                            , function () {
                                //  alertify.error('Cancelado')

                            });
                     
                    })
                    input2.addEventListener("keyup", function (event) {
                        // Number 13 is the "Enter" key on the keyboard
                        if (event.keyCode === 13) {
                            // Cancel the default action, if needed
                            event.preventDefault();
                            
                            if ($("#cantidad" + count).val() != "") {
                               
                                alertify.confirm('Confirmar Accion', 'Esta seguro?', function () {
                                    updateDetalle(rowIndice);
                                }
                                    , function () {
                                        //  alertify.error('Cancelado')

                                    });
                            } else {
                                $("#cantidad" + count).val(c)
                                alertify.alert("el campo no puede estar vacio")
                            }

                        }
                    });
                   
                   
                   
                    var c = $("#precio" + count).val()
                    c=c.replace("C$ ","").replace(",","")
                    var input3 = document.getElementById("precio" + count);
                    input3.addEventListener("keyup", function (event) {
                        // Number 13 is the "Enter" key on the keyboard
                        if (event.keyCode === 13) {
                            // Cancel the default action, if needed
                            event.preventDefault();

                            if ($("#precio" + count).val().replace("C$ ", "").replace(",", "") > 0) {

                                alertify.confirm('Confirmar Accion', 'Esta seguro?', function () {
                                   updateDetalle(rowIndice);
                                   // alertify.error($("#precio" + count).val().replace("C$ ", "").replace(",", ""))
                                }
                                    , function () {
                                        //  alertify.error('Cancelado')

                                    });
                            } else {
                                $("#cantidad" + count).val(c)
                                alertify.alert("el campo no puede ser menor o igual a 0")
                            }

                        }
                    });
                    $("#cantidad" + count).inputmask('integer', { min: 1, max: 1000000000 })
                    $("#precio" + count).inputmask({ alias: "currency", prefix: 'C$ ' })
                    subTotal = subTotal + Math.abs(jsonData.precioUnitario * jsonData.cantidad)

                }
                selectRefresh();  
                iva = Math.abs(subTotal * 0.15);
                total = subTotal + Math.abs(subTotal * 0.15)
                $("#subtotal").val(subTotal);
                $("#iva").val(iva);
                $("#total").val(total);
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
function loadUpdatePaquete(count) {
    for (var i = 0; i < Idpaquete.length; i++) {
        $('#paquete' + count).append($("<option />").val(Idpaquete[i]).text(paquete[i]));
        // paquete.push(jsonData.desTipoPaquete);
    }
}
function updateDetalle(rowIndice) {
    var arrayDeCadenas = idFactura.split("-");
    var precio = $("#precio" + rowIndice).val()
    precio = precio.replace("C$ ", "").replace(",", "").replace(".00","")
    $.ajax({
        url: urlUpdateDetalle,
        type: 'POST',
        dataType: 'json',
        data: {
            KeyFactura: arrayDeCadenas[1], keyFacturaDetalle: $("#keyFactura" + rowIndice).val(), Cantidad: $("#cantidad" + rowIndice).val(), PrecioUnitario: precio,
            KeyTipoPaquete: $('#paquete' + rowIndice).val()        },
        success: function (data, textStatus, xhr) {
            var outObjA = JSON.parse(JSON.stringify(data));
            $("#subtotal").val(outObjA[0].subtotal)
            $("#total").val(outObjA[0].total)
            $("#iva").val(outObjA[0].iva)
            LoadDetalle()
            alertify.alert("Registro Modificado")
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log('Error in Operation');


        }, complete: function () {

        }
    });
}
function getMunicipio() {
    $('#Origen').html("");
    $('#Destino').html("");

 

    $.ajax({
        url: urlMunicipios, // Url
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
                   
                    $('#Origen').append($("<option />").val(jsonData.keyMunicipio).text(jsonData.descripcionMun));
                    $('#Destino').append($("<option />").val(jsonData.keyMunicipio).text(jsonData.descripcionMun));
                    destino.push(jsonData.descripcionMun);
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

$("#modalCrear").click(function () {
    $("#modalFactura").modal("show")
});

$("#btnCrear").click(function () {

    const d = new Date($("#fechaElaboracion").val())
    console.log()
    $.ajax({
        url: urlInsertFactura, // Url
        data: {
            KeyCliente: $("#Cliente").val(), IdEmpresa: idEmpresa, IdSucursal: idSucursal,
            FechaElaboracion: d.toISOString().split('T')[0], UserName: Username
        },
        type: "post" // Verbo HTTP
       
    })
        // Se ejecuta si todo fue bien.
        .done(function (result) {
           
            if (result != null) {

                if (result == "1") {

                    alertify.alert("Factura creada")
                    $("#modalFactura").modal("hide");
                  
                    loadTable();
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
});
function loadCiente() {
    $('#Cliente').html("");

    $.ajax({
        url: urlGetCliente, // Url
        data: "",
        type: "post"  // Verbo HTTP
    })
        // Se ejecuta si todo fue bien.
        .done(function (result) {
          //  console.log(result)
            if (result != null) {

                var outObjA = JSON.parse(JSON.stringify(result));

                for (var i = 0; i < outObjA.length; i++) {
                    var jsonData = outObjA[i];
                    $('#Cliente').append($("<option />").val(jsonData.idCliente).text(jsonData.nombreComercial));

                }
            }
        })

        .fail(function (xhr, status, error) {
          
        })
      
        .always(function () {

        });

}


function loadEstados(id,estado) {
   

    $.ajax({
        url: urlGetEstado, // Url
        data: "",
        type: "post"  // Verbo HTTP
    })
        // Se ejecuta si todo fue bien.
        .done(function (result) {
         ///   console.log(result)
            if (result != null) {

                var outObjA = JSON.parse(JSON.stringify(result));

                for (var i = 0; i < outObjA.length; i++) {
                    var jsonData = outObjA[i];
                    $('#estado' + id).append($("<option />").val(jsonData.keyFacturaEstatus).text(jsonData.descripcionEstatus));
                }
                $('#estado' + id).val(estado);
                $('#estado' + id).trigger('change');
                $("#estado" + id).change(function () {
                    alertify.confirm('Confirmar Accion', 'Esta seguro?', function () {
                        $.ajax({
                            url: urlChangeEstado, // Url
                            data: {
                                IdOtdetalle: idDetalle
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

                            $('#estado' + id).val(estado);
                            $('#estado' + id).trigger('change');
                        });
                   
                })
            }
        })

        .fail(function (xhr, status, error) {

        })

        .always(function () {

        });

}
function loadTable() {
    $("#example").DataTable({
       
        "keys": {
            "clipboard": false
        },
        "ajax": {
            "url": urlLoad,
            "method": 'POST', //usamos el metodo POST
            "data": { id: $("#txt_buscar").val() }, //enviamos opcion 4 para que haga un SELECT
            "dataSrc": ""
        },
         "bDeferRender": true,
        "columns": [
            { "data": "keyFactura" },
            { "data": "noFactura" },
            { "data": "nombreComercial" },
            { "data": "fechaElaboracion" },
            {
                "data": null,
                "className": 'dt-body-left1',
                "render": function (data, type, full, meta) {
                    var opcion = '<select class="form-control select2bs4" style="width: 100%;" id="estado'+full.keyFactura+'"> </select >';
                  //  console.log(type)
                    if (type === 'filter') {
                        
                        loadEstados(full.keyFactura, full.keyFacturaEstatus)
                      
                    }

                   
                    selectRefresh(); 
                   
                    return opcion;
                   
                }
            },
            {
                "data": null,
                "className": 'dt-body-left',
                "render": function (data, type, full, meta) {
                    if (full.keyFacturaEstatus == 1) {
                        let str = full.nombreComercial;
                        var name=str.replace('"','');
                        //console.log(full.nombreComercial)
                        var opciones = " <div class='text-left'><div class='btn-group'><button class='btn  btn-sm btn-warning' style=' color: white'  onclick =updateEstado('" + full.idOrdenTrabajo + "');> <i class='material-icons'>Cerrar Orden</i><button class='btn  btn-sm' style='background: #014377; color: white' onclick =verDetalle('" + full.keyFactura + "','" + encodeURIComponent(full.nombreComercial) + "','" + full.fechaElaboracion + "','" + full.keyFacturaEstatus +"');> <i class='material-icons'>Ver Detalle</i></div></div>";
                    } else if (full.keyFacturaEstatus) {
                        var opciones = " <div class='text-left'><div class='btn-group'><button class='btn  btn-sm' style='background: #014377; color: white' onclick =verOrden('" + full.idOrdenTrabajo + "');> <i class='material-icons'>Ver Orden</i></button ></div></div>";
                    }
                    return opciones
                }
            }
        ],
        "destroy": true, "scrollX": "200%",
        "scrollY": "350px","select":true,
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
            "data": { idOrdenTrabajo: "" }, //enviamos opcion 4 para que haga un SELECT
            "dataSrc": ""
        },
        "columns": [
            {
                "data": null,
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

function verDetalle(idFactura,cliente,fecha,estado) {

    window.location.href = '/Factura/FacturaDetalle?id=' + idFactura+'&cliente='+cliente+'&fecha='+fecha+'&estado='+estado;
}


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

                    $('#tipoPaquete').append($("<option />").val(jsonData.idTipoPaquete).text(jsonData.desTipoPaquete));
                    paquete.push(jsonData.desTipoPaquete);
                    Idpaquete.push(jsonData.idTipoPaquete);

                    //  console.log(jsonData.descripcionDep)
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