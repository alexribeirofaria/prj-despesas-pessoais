USE `DespesasPessoaisDB`;

INSERT INTO
  `ImagemPerfilUsuario` (`Id`, `Url`, `Name`, `Type`, `UsuarioId`)
VALUES
  (
    2,
    'https://bucket-usuario-perfil.s3.amazonaws.com/perfil-usuarioId-1-20230703',
    'perfil-usuarioId-1-20230702',
    'jpg',
    1
  ),
  (
    3,
    'https://bucket-usuario-perfil.s3.amazonaws.com/perfil-usuarioId-2-20230907',
    'perfil-usuarioId-2-20230719',
    'png',
    2
  );