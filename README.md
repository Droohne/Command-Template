Практика «Отмена»

Продолжайте работу в том же проекте LimitedSizeStack.

Если вы запустите проект на исполнение, то увидите окно приложения, в котором можно добавлять новые дела и удалять уже существующие. Однако кнопка "Отмена" пока не работает. Ваша задача — сделать так, чтобы эта кнопка отменяла последнее действие пользователя.

Изучите класс ListModel — в нём реализована логика работы кнопок в приложении.

Реализуйте методы Undo и CanUndo. Для этого нужно хранить историю последних undoLimit действий удаления/добавления. Используйте для этого класс LimitedSizeStack из прошлой задачи. Его не нужно включать в отправляемый на проверку файл, считайте, что этот класс уже есть в проекте.

    Метод Undo отменяет последнее действие из истории.
    Метод CanUndo возвращает true, если на данный момент история действий не пуста, то есть если вызов Undo будет корректным. Иначе метод должен вернуть false.

Проверить корректность своего решения можно на модульных тестах из класса ListModel_Should и ListModel_PerformanceTest.

Если хотите, можете воспользоваться классическим объектно-ориентированным шаблоном Команда. Однако для сдачи данной задачи, точно следовать этому шаблону необязательно.
