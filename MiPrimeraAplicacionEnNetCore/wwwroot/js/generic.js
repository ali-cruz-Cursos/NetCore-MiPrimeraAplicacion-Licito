function mostrarModal(titulo = "¿Quieres agregar el nuevo registro?", texto = "Este registro se insertará a la base de datos." ) {
    return Swal.fire({
        title: titulo,
        text: texto,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Agregar!'
    })
}