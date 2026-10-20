using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentPerformanceApp
{
    // Описом клас для бази даних успішності студентів (Варіант 9)
    public class StudentPerformance
    {
        // Закриті поля класу (інкапсуляція)
        private string fullName;
        private string recordBookNumber;
        private int[] grades; // Оцінки з 5 предметів за 100-бальною шкалою

        // Конструктор без параметрів
        public StudentPerformance()
        {
            fullName = "Невідомо";
            recordBookNumber = "000000";
            grades = new int[5] { 0, 0, 0, 0, 0 };
        }

        // Конструктор з параметрами
        public StudentPerformance(string fullName, string recordBookNumber, int[] grades)
        {
            FullName = fullName;
            RecordBookNumber = recordBookNumber;
            Grades = grades;
        }

        // Властивість для Прізвища та імені
        public string FullName
        {
            get { return fullName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Прізвище та ім'я не можуть бути порожніми.");
                fullName = value.Trim();
            }
        }

        // Властивість для Номера залікової книжки
        public string RecordBookNumber
        {
            get { return recordBookNumber; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Номер залікової книжки не може бути порожнім.");
                recordBookNumber = value.Trim();
            }
        }

        // Властивість для Масиву оцінок з перевіркою коректності
        public int[] Grades
        {
            get { return grades; }
            set
            {
                if (value == null || value.Length != 5)
                    throw new ArgumentException("Масив повинен містити рівно 5 оцінок.");

                foreach (var grade in value)
                {
                    // Перевірка діапазону 100-бальної шкали
                    if (grade < 0 || grade > 100)
                        throw new FormatException("Оцінка повинна бути в діапазоні від 0 до 100.");
                }
                grades = value;
            }
        }

        // Метод для обчислення середнього бала студента
        public double GetAverageGrade()
        {
            return grades.Average();
        }

        // Перевизначення ToString для зручного виведення
        public override string ToString()
        {
            string gradesStr = string.Join(", ", grades);
            return $"{FullName,-20} | Залік: {RecordBookNumber,-8} | Оцінки: [{gradesStr}] | Середній бал: {GetAverageGrade(),5:F2}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Налаштування кодування консолі для української мови
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== ЛАБОРАТОРНА РОБОТА №4 - ЗАВДАННЯ 1 (ВАРІАНТ 9) ===");
            Console.WriteLine("База даних: Успішність групи студентів\n");

            try
            {
                // Створюємо масив/колекцію з 10 записів студентів
                List<StudentPerformance> students = new List<StudentPerformance>
                {
                    new StudentPerformance("Петренко Іван", "ЗК-101", new int[] { 85, 90, 78, 92, 88 }),
                    new StudentPerformance("Коваленко Марія", "ЗК-102", new int[] { 95, 98, 92, 96, 94 }),
                    new StudentPerformance("Шевченко Тарас", "ЗК-103", new int[] { 60, 65, 70, 58, 62 }),
                    new StudentPerformance("Бойко Олена", "ЗК-104", new int[] { 75, 80, 82, 78, 76 }),
                    new StudentPerformance("Мельник Андрій", "ЗК-105", new int[] { 50, 55, 60, 52, 58 }),
                    new StudentPerformance("Кравченко Анна", "ЗК-106", new int[] { 90, 88, 92, 95, 89 }),
                    new StudentPerformance("Поліщук Максим", "ЗК-107", new int[] { 70, 72, 68, 74, 71 }),
                    new StudentPerformance("Ткаченко Юлія", "ЗК-108", new int[] { 82, 85, 88, 84, 86 }),
                    new StudentPerformance("Гнатюк Олег", "ЗК-109", new int[] { 65, 60, 55, 62, 58 }),
                    new StudentPerformance("Лисенко Вікторія", "ЗК-110", new int[] { 96, 94, 98, 95, 97 })
                };

                // Впорядкування записів у порядку спадання середньої оцінки (вимога варіанта)
                var sortedStudents = students.OrderByDescending(s => s.GetAverageGrade()).ToList();

                // Виведення результатів на екран у формі таблиці
                Console.WriteLine("--- ТАБЛИЦЯ УСПІШНОСТІ СТУДЕНТІВ (За спаданням середнього бала) ---");
                Console.WriteLine(new string('-', 90));
                Console.WriteLine($"{"Прізвище та ім'я",-20} | {"№ заліковки",-10} | {"Оцінки з 5 предметів",-22} | {"Середній бал",-12}");
                Console.WriteLine(new string('-', 90));

                foreach (var student in sortedStudents)
                {
                    string gradesStr = string.Join(", ", student.Grades);
                    Console.WriteLine($"{student.FullName,-20} | {student.RecordBookNumber,-10} | {gradesStr,-22} | {student.GetAverageGrade(),-12:F2}");
                }
                Console.WriteLine(new string('-', 90));

                // Визначення відсотка студентів, середній бал яких відповідає оцінкам “добре” та “відмінно”
                // (За шкалою: "добре" та "відмінно" відповідає середньому балу від 75 і вище)
                int goodAndExcellentCount = sortedStudents.Count(s => s.GetAverageGrade() >= 75.0);
                double percentage = (double)goodAndExcellentCount / sortedStudents.Count * 100;

                Console.WriteLine($"\nКількість студентів із середнім балом рівня «добре» та «відмінно» (>= 75): {goodAndExcellentCount} з {sortedStudents.Count}");
                Console.WriteLine($"Відсоток таких студентів: {percentage:F2}%");

                // Демонстрація спрацювання виключення при введенні некоректних даних (наприклад, оцінка > 100)
                Console.WriteLine("\n--- ТЕСТУВАННЯ МЕХАНІЗМУ ВИКЛЮЧЕНЬ ---");
                Console.WriteLine("Спроба встановити студенту оцінку 110 (поза межами 0-100)...");
                sortedStudents[0].Grades = new int[] { 110, 85, 90, 80, 75 };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ПЕРЕХОПЛЕНО ВИКЛЮЧЕННЯ]: {ex.Message}");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу для завершення програми...");
            Console.ReadKey();
        }
    }
}