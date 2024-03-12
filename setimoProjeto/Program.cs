using System;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Collections.Generic;
using System.Xml.Linq;

namespace atividadeDeFixacao
{
    public class Program
    {
        //Strings de Controle de armazenamento permanente
        static string DelimitadorInicio;
        static string DelimitadorFim;
        static string tagName;
        static string tagDoc;
        static string tagBirth;
        static string tagHouse;
        //Diretorio do arquivo permanente
        static string trueWay;
        //Estrutura de armazenamento de dados temporario
        public struct dateStruct
        {
            public string name;
            public string doc;
            public DateTime dateOfBirth;
            public UInt32 houseNumber;
        }
        //Estrutura de comparação de strings
        public enum comparison
        {
            success = 0,
            exit = 1,
            wrong = 2
        }
        //Método para exibição de mensagem
        public static void message(string messageinfo)
        {
            Console.WriteLine(messageinfo);
            Console.WriteLine("Precione qualquer tecla para continuar!");
            Console.ReadKey(true);
            Console.Clear();
        }
        //Método para introdução de string
        public static comparison inputString(ref string addString, string message)
        {
            comparison goBack;
            Console.Write(message);
            string temporary = Console.ReadLine();
            if ((temporary == "s") || (temporary == "S"))
            {
                goBack = comparison.exit;
            }
            else
            {
                addString = temporary;
                goBack = comparison.success;
            }
            Console.Clear();
            return goBack;
        }
        //Método para introdução de datetime
        public static comparison inputDateTime(ref DateTime addDateTime, string message)
        {
            comparison goBack;
            do
            {
                try
                {
                    Console.Write(message);
                    string temporary = Console.ReadLine();
                    if ((temporary == "s") || (temporary == "S"))
                    {
                        goBack = comparison.exit;
                    }
                    else
                    {
                        addDateTime = Convert.ToDateTime(temporary);
                        goBack = comparison.success;
                    }
                }
                catch (Exception wrong)
                {
                    Console.WriteLine("EXCEÇÃO: " + wrong.Message);
                    goBack = comparison.wrong;
                    Console.WriteLine("Precione qualquer tecla para continuar!");
                    Console.ReadKey(true);
                    Console.Clear();
                }
            } while (goBack == comparison.wrong);
            Console.Clear();
            return goBack;
        }
        //Método para introdução de unidades númericas
        public static comparison inputNumber(ref UInt32 addNumber, string message)
        {
            comparison goBack;
            do
            {
                try
                {
                    Console.Write(message);
                    string temporary = Console.ReadLine();
                    if ((temporary == "s") || (temporary == "S"))
                    {
                        goBack = comparison.exit;
                    }
                    else
                    {
                        addNumber = Convert.ToUInt32(temporary);
                        goBack = comparison.success;
                    }
                }
                catch (Exception wrong)
                {
                    Console.WriteLine("EXCEÇÃO: " + wrong.Message);
                    goBack = comparison.wrong;
                    Console.WriteLine("Precione qualquer tecla para continuar!");
                    Console.ReadKey(true);
                    Console.Clear();
                }
            } while (goBack == comparison.wrong);
            Console.Clear();
            return goBack;
        }
        //Método principal para cadastro
        public static comparison register(ref List<dateStruct> UserList)
        {
            dateStruct addRegister;
            addRegister.name = "";
            addRegister.doc = "";
            addRegister.dateOfBirth = new DateTime();
            addRegister.houseNumber = 0;
            if (inputString(ref addRegister.name, "Digite o nome ou (S) para sair: ") == comparison.exit)
                return comparison.exit;
            if (inputString(ref addRegister.doc, "Digite o documento ou (S) para sair: ") == comparison.exit)
                return comparison.exit;
            if (inputDateTime(ref addRegister.dateOfBirth, "Digite a data de nascimento DD/MM/AAAA ou (S) para sair: ") == comparison.exit)
                return comparison.exit; ;
            if (inputNumber(ref addRegister.houseNumber, "Digite o número da casa ou (S) para sair: ") == comparison.exit)
                return comparison.exit; ;
            UserList.Add(addRegister);
            recordsDate(trueWay, UserList);
            return comparison.success;
        }
        //Método para gravar dados permanentemente
        public static void recordsDate(string way, List<dateStruct> UserList)
        {
            string fileContent = "";
            try
            {
                foreach (dateStruct line in UserList)
                {
                    fileContent += DelimitadorInicio + "\n";
                    fileContent += tagName + line.name + "\n";
                    fileContent += tagDoc + line.doc + "\n";
                    fileContent += tagBirth + line.dateOfBirth.ToString("dd/MM/yyyy") + "\n";
                    fileContent += tagHouse + line.houseNumber + "\n";
                    fileContent += DelimitadorFim + "\n";
                }
                File.WriteAllText(way, fileContent);
            }
            catch (Exception wrong)
            {
                Console.WriteLine("EXCEÇÃO: " + wrong.Message);
                Console.WriteLine("Precione qualquer tecla para continuar!");
                Console.ReadKey(true);
                Console.Clear();
            }
        }
        //Método de Carregamento de dados
        public static void loadDate(string way, ref List<dateStruct> UserList)
        {
            try
            {
                if (File.Exists(way))
                {
                    string[] arrayFile = File.ReadAllLines(way);
                    dateStruct dateFile;
                    dateFile.name = "";
                    dateFile.doc = "";
                    dateFile.dateOfBirth = new DateTime();
                    dateFile.houseNumber = 0;
                    foreach (string line in arrayFile)
                    {
                        if (line.Contains(DelimitadorInicio))
                            continue;
                        if (line.Contains(DelimitadorFim))
                            UserList.Add(dateFile);
                        if (line.Contains(tagName))
                            dateFile.name = line.Replace(tagName, "");
                        if (line.Contains(tagDoc))
                            dateFile.doc = line.Replace(tagDoc, "");
                        if (line.Contains(tagBirth))
                            dateFile.dateOfBirth = Convert.ToDateTime(line.Replace(tagBirth, ""));
                        if (line.Contains(tagHouse))
                            dateFile.houseNumber = Convert.ToUInt32(line.Replace(tagHouse, ""));
                    }
                }
            }
            catch (Exception wrong)
            {
                Console.WriteLine("EXCEÇÃO: " + wrong.Message);
                Console.WriteLine("Precione qualquer tecla para continuar!");
                Console.ReadKey(true);
                Console.Clear();
            }
        }
        //Método para buscar usuário
        public static void searchUserByTheDoc(List<dateStruct> UserList)
        {
            Console.Clear();
            Console.Write("Digite o documento ou (S) para sair: ");
            string temporary = Console.ReadLine();
            if ((temporary == "s") || (temporary == "S"))
                return;
            else
            {
                Console.Clear();
                List<dateStruct> temporaryUserList = UserList.Where(x => x.doc == temporary).ToList();
                if (temporaryUserList.Count > 0)
                {
                    foreach (dateStruct line in temporaryUserList)
                    {
                        Console.WriteLine(DelimitadorInicio);
                        Console.WriteLine(tagName + line.name);
                        Console.WriteLine(tagDoc + line.doc);
                        Console.WriteLine(tagBirth + line.dateOfBirth.ToString("dd/MM/yyyy"));
                        Console.WriteLine(tagHouse + line.houseNumber);
                        var anoatual = DateTime.Now;
                        int idade = anoatual.Year - line.dateOfBirth.Year;
                        Console.WriteLine("Idade: " + idade + " anos");
                        Console.WriteLine(DelimitadorFim);
                    }
                }
                else
                {
                    Console.WriteLine("Nenhum documento " + temporary + " encontrado");  
                }
                message("");
            }
        }
        //Método de deletar usuário
        public static void deleteUserByTheDoc(ref List<dateStruct> userList)
        {
            Console.Write("Digite o documento ou (S) para sair: ");
            string temporary = Console.ReadLine();
            bool deletedUser = false;
            if ((temporary == "s") || (temporary == "S"))
                return;
            else
            {
                Console.Clear();
                List<dateStruct> temporaryUserList = userList.Where(x => x.doc == temporary).ToList();
                if (temporaryUserList.Count > 0)
                {
                    foreach (dateStruct line in temporaryUserList)
                    { 
                        userList.Remove(line);
                        deletedUser = true;
                    }
                    if (deletedUser)
                    {
                        recordsDate(trueWay, userList);
                        Console.WriteLine(temporaryUserList.Count + " usuário(a) de documento " + temporary + " excluido(a)");
                    }
                }
                else
                {
                    Console.WriteLine("Nenhum documento " + temporary + " encontrado");
                }
                message("");
            }
        }
        //Método para apresentação básica de sistema
        public static void presentation(ref List<dateStruct> UserList)
        {
            foreach (dateStruct line in UserList)
            {
                Console.WriteLine(DelimitadorInicio);
                Console.WriteLine(tagName + line.name);
                Console.WriteLine(tagDoc + line.doc);
                Console.WriteLine(tagBirth + line.dateOfBirth.ToString("dd/MM/yyyy"));
                var anoatual = DateTime.Now;
                int idade = anoatual.Year - line.dateOfBirth.Year;
                Console.WriteLine("Idade: " + idade + " anos");
                Console.WriteLine(tagHouse + line.houseNumber);
                Console.WriteLine(DelimitadorFim);
                Console.WriteLine();
            }
        }
        public static void Main(string[] args)
        {
            //Array List de armazenamento e organização temporaria
            List<dateStruct> UserList = new List<dateStruct>();

            //Caracterização de variavel permanente
            DelimitadorInicio = "Início _ ";
            tagName = "Nome: ";
            tagDoc = "Documento: ";
            tagBirth = "Data: ";
            tagHouse = "Nº: ";
            DelimitadorFim = "Fim _ ";

            //Chamada do diretorio do arquivo permanente
            trueWay = "baseDeDados.txt";

            //Chamada do loadDate
            loadDate(trueWay, ref UserList);

            string option;
            do
            {
                Console.WriteLine("Precione [C] para Cadastrar um usuário");
                Console.WriteLine("-");
                Console.WriteLine("Precione [B] para Buscar um usuário");
                Console.WriteLine("-");
                Console.WriteLine("Precione [E] para Excluir um usuário");
                Console.WriteLine("-");
                Console.WriteLine("Precione [M] para Mostrar os dados já cadastrados");
                Console.WriteLine("-");
                Console.WriteLine("Precione [S] para Sair");
                Console.WriteLine("-");
                option = Console.ReadKey(true).KeyChar.ToString().ToUpper();
                //Chamada de cadastramento
                if (option == "C")
                {
                    Console.Clear();
                    register(ref UserList);
                    message("O cadastro foi executado!");
                }
                //Chamada de busca de usuário
                else if (option == "B")
                {
                    Console.Clear();
                    searchUserByTheDoc(UserList);
                }
                //Chamada de excluir usuário
                else if (option == "E")
                {
                    Console.Clear(); 
                    deleteUserByTheDoc(ref UserList);
                }
                //Chamada de apresentação de cadastro
                else if (option == "M")
                {
                    Console.Clear();
                    //Sem arquivo salvo
                    if (UserList.Count == 0)
                    {
                        message("Não há arquivos salvo!");
                    }
                    //Com arquivo
                    else
                    {
                        presentation(ref UserList);
                        message("A apresentação foi executado!");
                    }
                }
                //Chamada de fim
                else if (option == "S")
                {
                    Console.Clear();
                    message("A execução será finalizada!");
                    break;
                }
                else
                {
                    Console.Clear();
                    message("Não há está opção!");
                }
            } while (option != "s");
        }
    }
}