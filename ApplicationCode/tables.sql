-- Таблиця користувачів
CREATE TABLE Users (
    user_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL,
    gender VARCHAR(10) NOT NULL,
    age INT NOT NULL,
    height DECIMAL(5, 2) NOT NULL,
    weight DECIMAL(5, 2) NOT NULL,
    allergies TEXT, 
    goal VARCHAR(50),
    registration_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_login TIMESTAMP
);

-- Таблиця прийомів їжі
CREATE TABLE FoodIntake (
    food_intake_id SERIAL PRIMARY KEY,
    user_id INT REFERENCES Users(user_id) ON DELETE CASCADE,
    intake_time TIMESTAMP NOT NULL,
    meal_type VARCHAR(50) NOT NULL, --сніданок, обід, вечеря
    food_items TEXT NOT NULL, -- Список продуктів, можна зберігати як текст
    total_calories DECIMAL(10, 2) NOT NULL
);

-- Таблиця продуктів
CREATE TABLE Food (
    food_id SERIAL PRIMARY KEY,
    food_name VARCHAR(100) NOT NULL,
    calories_per_100g DECIMAL(10, 2) NOT NULL,
    allergens TEXT -- Продукти можуть мати різні алергени
);

-- Таблиця цілей користувача
CREATE TABLE UserGoals (
    goal_id SERIAL PRIMARY KEY,
    user_id INT REFERENCES Users(user_id) ON DELETE CASCADE,
    goal_type VARCHAR(50) NOT NULL, -- схуднути, набрати вагу і т.д
    target_weight DECIMAL(5, 2) NOT NULL,
    start_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    end_date TIMESTAMP
);

-- Таблиця статистики
CREATE TABLE Statistics (
    stats_id SERIAL PRIMARY KEY,
    user_id INT REFERENCES Users(user_id) ON DELETE CASCADE,
    stat_date DATE NOT NULL,
    avg_calories DECIMAL(10, 2),
    total_meals INT,
    achievement TEXT -- Опис досягнень або шлях до цілей
);

-- Таблиця сповіщень
CREATE TABLE Notifications (
    notification_id SERIAL PRIMARY KEY,
    user_id INT REFERENCES Users(user_id) ON DELETE CASCADE,
    notification_type VARCHAR(50) NOT NULL, -- Тип сповіщення (їжа, вода, медикаменти)
    notification_time TIMESTAMP NOT NULL,
    status BOOLEAN DEFAULT TRUE -- Увімкнене або вимкнене
);

-- Таблиця статей
CREATE TABLE Articles (
    article_id SERIAL PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    description TEXT NOT NULL,
    topic VARCHAR(100) NOT NULL,
    publication_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Таблиця збережених статей користувачами
CREATE TABLE SavedArticles (
    saved_article_id SERIAL PRIMARY KEY,
    user_id INT REFERENCES Users(user_id) ON DELETE CASCADE,
    article_id INT REFERENCES Articles(article_id) ON DELETE CASCADE
);

-- Таблиця рекомендацій фізичних вправ
CREATE TABLE ExerciseRecommendations (
    recommendation_id SERIAL PRIMARY KEY,
    exercise_type VARCHAR(100) NOT NULL,
    description TEXT NOT NULL,
    calories_burned DECIMAL(10, 2) NOT NULL
);
