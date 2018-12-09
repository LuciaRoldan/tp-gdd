
create procedure actualizarCompraFactura_sp (@id_factura int, @id_compra int) as begin
	update Compras set id_factura = @id_factura where id_compra = @id_compra
end
