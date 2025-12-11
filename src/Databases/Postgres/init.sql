-- Create database
-- CREATE DATABASE dbbroker;

-- Connect to the new database
\c dbbroker

-- Create the tables
CREATE TABLE customers (
    id UUID PRIMARY KEY,
    name VARCHAR(250) NOT NULL,
    birthday DATE,
    orders_count INT,
    created_at TIMESTAMPTZ NOT NULL,
    created_by VARCHAR(50) NOT NULL,
    modified_at TIMESTAMPTZ,
    modified_by VARCHAR(50)
);

CREATE TABLE customers_notes_status (
    id SERIAL PRIMARY KEY,
    status VARCHAR(50)
);

CREATE TABLE customers_notes (
    id UUID PRIMARY KEY,
    customer_id UUID,
    status_id INT,
    note_content VARCHAR(50) NOT NULL,
    FOREIGN KEY (customer_id) REFERENCES customers (id),
    FOREIGN KEY (status_id) REFERENCES customers_notes_status (id)
);

CREATE TABLE order_status (
    id SERIAL PRIMARY KEY,
    status VARCHAR(50) NOT NULL
);

CREATE TABLE orders (
    id UUID PRIMARY KEY,
    customer_id UUID NOT NULL,
    status_id INT,
    created_at TIMESTAMPTZ NOT NULL,
    created_by VARCHAR(50) NOT NULL,
    modified_at TIMESTAMPTZ,
    modified_by VARCHAR(50),
    FOREIGN KEY (customer_id) REFERENCES customers (id),
    FOREIGN KEY (status_id) REFERENCES order_status (id)
);

CREATE TABLE orders_notes (
    id SERIAL PRIMARY KEY,
    note_content VARCHAR(1024) NOT NULL
);

CREATE TABLE products (
    id UUID PRIMARY KEY,
    product_name VARCHAR(50) NOT NULL,
    price DECIMAL(10, 2) NOT NULL
);

CREATE TABLE orders_products (
    id UUID PRIMARY KEY,
    order_id UUID NOT NULL,
    product_id UUID NOT NULL,
    FOREIGN KEY (order_id) REFERENCES orders (id),
    FOREIGN KEY (product_id) REFERENCES products (id)
);

CREATE TABLE promotions (
    id SERIAL PRIMARY KEY,
    title VARCHAR(50) NOT NULL,
    expiration TIMESTAMPTZ NOT NULL
);

CREATE TABLE promotions_enrollments (
    id SERIAL PRIMARY KEY,
    customer_id UUID NOT NULL,
    promotion_id INT NOT NULL,
    FOREIGN KEY (customer_id) REFERENCES customers (id),
    FOREIGN KEY (promotion_id) REFERENCES promotions (id)
);

-- SEEDING DATA

-- Customers >>>
INSERT INTO customers (id, name, birthday, orders_count, created_at, created_by)
VALUES ('123e4567-e89b-12d3-a456-426614174000', 'John Doe', '1980-01-01', 5, NOW(), 'system');

INSERT INTO customers (id, name, birthday, orders_count, created_at, created_by)
VALUES ('223e4567-e89b-12d3-a456-426614174001', 'Jane Smith', '1990-02-02', 3, NOW(), 'system');

INSERT INTO customers (id, name, birthday, orders_count, created_at, created_by)
VALUES ('323e4567-e89b-12d3-a456-426614174002', 'Alice Johnson', '1985-03-03', 8, NOW(), 'system');
-- <<< Customers

-- Customers Notes Status >>>
INSERT INTO customers_notes_status (status)
VALUES ('Enabled');

INSERT INTO customers_notes_status (status)
VALUES ('Disabled');
-- <<< Customers Notes Status

-- Products >>>
INSERT INTO products (id, product_name, price)
VALUES ('11111111-1111-1111-1111-111111111111', 'Product A', 10.00);

INSERT INTO products (id, product_name, price)
VALUES ('22222222-2222-2222-2222-222222222222', 'Product B', 20.00);

INSERT INTO products (id, product_name, price)
VALUES ('33333333-3333-3333-3333-333333333333', 'Product C', 30.00);
-- <<< Products

-- Order Status >>>
INSERT INTO order_status (status)
VALUES ('New');

INSERT INTO order_status (status)
VALUES ('Processing');

INSERT INTO order_status (status)
VALUES ('Shipped');

INSERT INTO order_status (status)
VALUES ('Delivered');

INSERT INTO order_status (status)
VALUES ('Cancelled');
-- <<< Order Status

