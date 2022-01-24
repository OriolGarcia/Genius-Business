
USE etddatabase;
CREATE TABLE GnB_Empresas ( 

	Empresa_idEmpresa bigint PRIMARY KEY IDENTITY (1, 1),
	Nombre_Empresa VARCHAR(100),
	Emailprincipal VARCHAR(200),
	NumTel VARCHAR(200),
	ContraseñaEmpresa VARCHAR(200),
	Pais VARCHAR(200)
);
CREATE TABLE GnB_EmpresasLicencias ( 

	Emlc_idlicencia bigint Not Null,
	Emlc_idEmpresa bigint Not Null,
	Primary Key (Emlc_idlicencia, Emlc_idEmpresa),
);
CREATE TABLE GbB_Tiendas ( 
	tienda_idempresa bigint NOT NULL,
	tienda_id BIGINT PRIMARY KEY IDENTITY (1, 1),
	tienda_NombreTienda VARCHAR(100),
	tienda_Direccion VARCHAR(200),
	tienda_Población VARCHAR(200),
	tienda_Pais VARCHAR(100)
);
CREATE TABLE GbB_productos ( 

	producto_idempresa bigint NOT NULL,
    producos_id BIGINT PRIMARY KEY IDENTITY (1, 1),
    producto_fecha DATETIME NOT NULL,
	producto_tiendaid BIGINT NOT NULL,
	poducto_costeunidad Bigint,
    producto_preciounidad Decimal NOT NULL,
	producto_ivaporcent Decimal Not Null,
	producto_imagen varchar(200),
);


Create table GbB_cliente (
cliente_idempresa bigint NOT NULL,
cleinte_id BIGINT PRIMARY KEY IDENTITY (1, 1),
venta_fecha DATETIME NOT NULL,
venta_nombre VARCHAR(200) NOT NULL,
venta_apellidos VARCHAR(200) NOT NULL,
venta_telefono VARCHAR(200) NOT NULL,
venta_email VARCHAR(200) NOT NULL,
venta_redessociale VARCHAR(1000) NOT NULL,
);
CREATE TABLE GbB_ventas ( 

	venta_idempresa bigint NOT NULL,
    venta_id BIGINT PRIMARY KEY IDENTITY (1, 1),
    venta_fecha DATETIME NOT NULL,
	venta_tiendaid BIGINT NOT NULL,
    venta_importe Decimal NOT NULL,
	venta_idcliente bigint
);

CREATE TABLE GbB_ventasreales ( 

	ventareal_idempresa bigint NOT NULL,
    ventareal_ventas_id BIGINT PRIMARY KEY IDENTITY (1, 1),
	ventareal_idproducto bigint,
	ventareal_cantidad  int,
	ventareal_costeunidad Decimal(15,2),
	ventarela_costetotal Decimal(15,2),
	ventareal_preciounidad DEcimal (15,2),
	ventareal_descuentounidad Decimal(15,2),
	ventareal_total Decimal(15,2),
	ventareal_ivaporcent Decimal(15,2),
	ventareal_ivaTotal Decimal(15,2),
	vantareal_totaliva Decimal(15,2),
    ventareal_fecha DATETIME NOT NULL,
);
Create table Paises(
paises_id BIGINT PRIMARY KEY IDENTITY (1, 1),
paises_pais VARCHAR(50),
paises_moneda VARCHAR(10)



);