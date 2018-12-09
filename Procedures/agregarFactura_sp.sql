
create procedure agregarFactura_sp (@razonSocial NVARCHAR(255), @total NUMERIC(18,2)) as begin
	declare @id_empresa int
	set @id_empresa = (select id_empresa from Empresas where razon_social = @razonSocial)
	insert into Facturas (id_empresa, fecha_facturacion, importe_total) values (@id_empresa, getdate(), @total)

	select top 1 id_factura from Facturas where id_empresa = @id_empresa and importe_total = @total order by fecha_facturacion desc
end



