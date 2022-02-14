ServiceCreditRequest

Сервис обработки заявок на кредит.
REST микросервис, паттерн репозиторий.

Краткое описание работы:
 CreditRequestController (api/application/create) получает запрос на кредит 
и возвращает Id заявки и номер договора,
CreditRequestManager преобразует входящий запрос в модель внутреннего слоя
и обращается к репозиториям для их сохранения.
 CreditRequestJob в заданные промежутки времени обращается к CreditRequestManager
для получения заявок на кредит которые не прошли скоринг-оценку и при помощи CreditRequestClient 
отправляет эти заявки скоринг-сервису.
 Скоринг сервис обрабатывает заявки и передаёт их CreditRequestController
по пути api/application/scoring/update.
 Пользователь сервиса может получить результаты скоринга по апи api/application/status.

Appsettings:
ConnectionStrings - строки подключения 
 DefaultConnection - строка подключения к бд
 Примечание: если бд пустая структура создаётся автоматически, сама бд уже должна быть создана
ScoringService - информация о скоринг-сервисе
 EvaluateUri - апи строка по которой передаются заявки для скоринга
 ScoringServiceUri - адрес скоринг-сервиса
Quartz - настройки для библиотеки quartz
 cronExpression - временной интервал для работы библиотеки quartz

Структура:

Client - http-клиент для передачи заявок на скоринг

Controllers - контроллеры для обработки входящих http запросов

Data - управление данными
 Repository - манипуляции с данными, CRUD
 Migrations - миграции  для создания структуры БД

Domain - middleware
 Managers - преобразование данных для слоёв приложения

Infrastructure - расширения для приложения
 Mapper - профайл для библиотеки automapper
 Extensions - расширения для класса startup

Jobs - схема для работы работы библиотеки quartz

Models - объекты данных
 Entities - модели для внутреннего использования
 Incoming - входящие запросы к сервису
 Outcoming - исходящие запросы от сервиса
