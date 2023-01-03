create table Product
(
  Id uniqueidentifier not null primary key,
  Code bigint not null unique,
  Status int not null,
  ImportedDate datetime,
  Name text,
  BarCode text,
  URL text,
  Quantity text,
  Categories text,
  Packaging text,
  Brands text,
  ImageURL text
);