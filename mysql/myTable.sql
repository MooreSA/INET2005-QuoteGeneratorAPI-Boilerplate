-- Host: localhost:3306
-- Generation Time: Sep 25, 2016 at 10:48 PM
-- Server version: 5.6.33
-- PHP Version: 5.6.20

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


CREATE TABLE IF NOT EXISTS quotes (
  id int NOT NULL AUTO_INCREMENT PRIMARY KEY,
  author varchar(100) NOT NULL,
  quote varchar(1000) NOT NULL,
  permalink varchar(100) NOT NULL,
  image varchar(100) NOT NULL
);

INSERT INTO `quotes` (`author`, `quote`, `permalink`, `image`) VALUES
('Albert Einstein', 'I have no special talent. I am only passionately curious.', 'albert-einstein', '1-albert-einstein.jpg'),
('Albert Einstein', 'I am thankful for all of those who said NO to me. It''s because of them I''m doing it.', 'albert-einstein', '2-albert-einstein.jpg'),
('Albert Einstein', 'I am not a product of my circumstances. I am a product of my decisions.', 'albert-einstein', '3-albert-einstein.jpg'),
('Albert Einstein', 'I think and that is all that I am.', 'albert-einstein', '4-albert-einstein.jpg'),
('Albert Einstein', 'I have not failed. I''ve just found 10,000 ways that won''t work.', 'albert-einstein', '5-albert-einstein.jpg'),
('Albert Einstein', 'I am not a product of my circumstances. I am a product of my decisions.', 'albert-einstein', '6-albert-einstein.jpg'),
('Albert Einstein', 'I think and that is all that I am.', 'albert-einstein', '7-albert-einstein.jpg'),
('Albert Einstein', 'I have not failed. I''ve just found 10,000 ways that won''t work.', 'albert-einstein', '8-albert-einstein.jpg'),
('Albert Einstein', 'I am not a product of my circumstances. I am a product of my decisions.', 'albert-einstein', '9-albert-einstein.jpg');