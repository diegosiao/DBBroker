-- Create database
-- CREATE DATABASE dbbroker;

-- Connect to the new database
\c dbbroker

-- Create the tables
CREATE TABLE customers (
    id UUID PRIMARY KEY,
    name VARCHAR(250) NOT NULL,
    birthday DATE,
    orders_total INT,
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
    product_name VARCHAR(50) NOT NULL
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

-- Data

INSERT INTO customers_notes_status (status)
VALUES ('Enabled');

INSERT INTO customers_notes_status (status)
VALUES ('Disabled');

