-- Create the tables
CREATE TABLE customers (
    id CHAR(36) PRIMARY KEY,
    name VARCHAR(250) NOT NULL,
    birthday DATE,
    orders_total INT,
    created_at DATETIME NOT NULL,
    created_by VARCHAR(50) NOT NULL,
    modified_at DATETIME,
    modified_by VARCHAR(50)
);

CREATE TABLE customers_notes_status (
    id INT AUTO_INCREMENT PRIMARY KEY,
    status VARCHAR(50)
);

CREATE TABLE customers_notes (
    id CHAR(36) PRIMARY KEY,
    customer_id CHAR(36),
    status_id INT,
    note_content VARCHAR(50) NOT NULL,
    FOREIGN KEY (customer_id) REFERENCES customers(id),
    FOREIGN KEY (status_id) REFERENCES customers_notes_status(id)
);

CREATE TABLE order_status (
    id INT AUTO_INCREMENT PRIMARY KEY,
    status VARCHAR(50) NOT NULL
);

CREATE TABLE orders (
    id CHAR(36) PRIMARY KEY,
    customer_id CHAR(36) NOT NULL,
    status_id INT,
    created_at DATETIME NOT NULL,
    created_by VARCHAR(50) NOT NULL,
    modified_at DATETIME,
    modified_by VARCHAR(50),
    FOREIGN KEY (customer_id) REFERENCES customers(id),
    FOREIGN KEY (status_id) REFERENCES order_status(id)
);

CREATE TABLE orders_notes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    note_content VARCHAR(1024) NOT NULL
);

CREATE TABLE products (
    id CHAR(36) PRIMARY KEY,
    product_name VARCHAR(50) NOT NULL
);

CREATE TABLE orders_products (
    id CHAR(36) PRIMARY KEY,
    order_id CHAR(36) NOT NULL,
    product_id CHAR(36) NOT NULL,
    FOREIGN KEY (order_id) REFERENCES orders(id),
    FOREIGN KEY (product_id) REFERENCES products(id)
);

CREATE TABLE promotions (
    id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(50) NOT NULL,
    expiration DATETIME NOT NULL
);

CREATE TABLE promotions_enrollments (
    id INT AUTO_INCREMENT PRIMARY KEY,
    customer_id CHAR(36) NOT NULL,
    promotion_id INT NOT NULL,
    FOREIGN KEY (customer_id) REFERENCES customers(id),
    FOREIGN KEY (promotion_id) REFERENCES promotions(id)
);
