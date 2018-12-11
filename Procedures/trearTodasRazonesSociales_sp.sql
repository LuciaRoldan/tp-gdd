
create procedure traerTodasRazonesSociales_sp as begin
	select distinct razon_social from Empresas
end
