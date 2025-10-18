-- Garante que o banco exista e está em uso
CREATE DATABASE IF NOT EXISTS `tst` DEFAULT CHARACTER SET utf8mb4;
USE `tst`;

-- Cria a tabela em minúsculas (como a aplicação consulta)
CREATE TABLE IF NOT EXISTS `modalidade` (
  `Id` INT AUTO_INCREMENT PRIMARY KEY,
  `Nome` VARCHAR(100) NOT NULL,
  `TurmaId` INT NULL,
  INDEX `IX_modalidade_TurmaId` (`TurmaId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Opcional: após confirmar o nome exato da tabela de Turmas, adicione a FK manualmente:
-- ALTER TABLE `modalidade`
--   ADD CONSTRAINT `FK_modalidade_Turma`
--   FOREIGN KEY (`TurmaId`) REFERENCES `Turma`(`Id`) ON DELETE SET NULL;
