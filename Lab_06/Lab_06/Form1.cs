using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_06
{
    enum request_type //перечисляем типы операций
    {
        ACNT, //узнать счет
        XCHG, //обмен валюты
        CRED, //выдать кредит
        CARD, //сделать карту
        NONE
    };
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n = int.Parse(textBox2.Text); //общее количество заявок
            List<Request> list_client = new List<Request>();
            int ID = 1;
            Random rnd = new Random();
            request_type operation = request_type.NONE;
            int start_time;
            int service_time = 0;
            int timer = 20;
            int dec = 0;
            int tot_reject_XCHG = 0;    //количество отказов первого типа
            int tot_reject_ACNT = 0;    //количетво отказов второго типа
            double tot_queue_count1 = 0;
            double tot_queue_count2 = 0;
            double avr_queue_count1 = 0;
            double avr_queue_count2 = 0;
            //генерируем заявки
            for (int i = 0; i < n; i++)
            {
                int type = rnd.Next(4);
                switch (type)
                {
                    case 0:
                        operation = request_type.ACNT;
                        dec = rnd.Next(-10, 10);
                        service_time = set_service_time(operation, dec);
                        break;
                    case 1:
                        operation = request_type.CARD;
                        dec = rnd.Next(-10, 10);
                        service_time = set_service_time(operation, dec);
                        break;
                    case 2:
                        operation = request_type.CRED;
                        dec = rnd.Next(-10, 10);
                        service_time = set_service_time(operation, dec);
                        break;
                    case 3:
                        operation = request_type.XCHG;
                        dec = rnd.Next(-10, 10);
                        service_time = set_service_time(operation, dec);
                        break;
                }
                start_time = timer + rnd.Next(-10, 10);
                list_client.Add(new Request(ID, start_time, service_time, true, operation));
                ID++;
                timer += 5;
            }
            foreach (Request client in list_client)
            {
                textBox1.Text += client.ID + " " + client.operation + " " + client.start_time + " " + "\r\n";
            }
            //создаем кассы
            ID = 1;
            List<Kassa> list_kassa = new List<Kassa>(7);
            for (int i = 0; i < 7; i++)
            {
                if (i == 0 || i == 1)
                { list_kassa.Add(new Kassa(ID, kassa_type.N12)); ID++; }
                else
                { list_kassa.Add(new Kassa(ID, kassa_type.N37)); ID++; }
            }
            foreach (Kassa kassir in list_kassa)
            {
                textBox3.Text += kassir.state + " " + kassir.k_type + "\r\n";
            }
            //создаем очереди для заявок
            int q_volume = int.Parse(textBox4.Text);
            Queue<Request> q12 = new Queue<Request>(q_volume);
            Queue<Request> q37 = new Queue<Request>(q_volume);

            List<Request> list_ready = new List<Request>(n);
            int global_time = 0;
            ///*

            while (list_ready.Count != n)
            {
                //освобождаем кассы
                foreach (Kassa kassir in list_kassa)
                {
                    if (kassir.state == operator_state.Occuped && (kassir.cl.start_time + kassir.cl.service_time) == global_time)
                    {
                        list_ready.Add(kassir.cl);
                        kassir.cl = null;
                        kassir.state = operator_state.Vacant;
                    }
                }
                //добавляем из очередей новые заявки в свободные кассы
                foreach (Kassa kassir in list_kassa)
                {
                    if (kassir.state == operator_state.Vacant && kassir.k_type == kassa_type.N12 && q12.Count != 0)
                    {
                        kassir.cl = q12.Peek();
                        kassir.state = operator_state.Occuped;
                        q12.Dequeue();
                    }
                    if (kassir.state == operator_state.Vacant && kassir.k_type == kassa_type.N37 && q37.Count != 0)
                    {
                        kassir.cl = q37.Peek();
                        kassir.state = operator_state.Occuped;
                        q37.Dequeue();
                    }

                }
                foreach (Request client in list_client)
                {
                    if (client.start_time == global_time)
                    {
                        foreach (Kassa kassir in list_kassa)
                        {
                            if (kassir.state == operator_state.Vacant && kassir.k_type == kassa_type.N12 && client.operation == request_type.XCHG)
                            {
                                kassir.cl = client;
                                kassir.state = operator_state.Occuped;
                                break;
                            }

                            if (kassir.state == operator_state.Vacant && kassir.k_type == kassa_type.N37)
                            {
                                if (client.operation == request_type.ACNT || client.operation == request_type.CARD || client.operation == request_type.CRED)
                                {
                                    kassir.cl = client;
                                    kassir.state = operator_state.Occuped;
                                    break;
                                }
                            }

                            if (kassir.state == operator_state.Occuped && kassir.ID == 2 && client.operation == request_type.XCHG)
                            {
                                if (q12.Count == q_volume)
                                {
                                    client.request_state = false;
                                    list_ready.Add(client);
                                    tot_reject_XCHG++;
                                }
                                else q12.Enqueue(client);
                                break;
                            }
                            if (kassir.state == operator_state.Occuped && kassir.ID == 7)
                            {
                                if (client.operation == request_type.ACNT || client.operation == request_type.CARD || client.operation == request_type.CRED)
                                {
                                    if (q37.Count == q_volume)
                                    {
                                        client.request_state = false;
                                        list_ready.Add(client);
                                        tot_reject_ACNT++;
                                    }
                                    else q37.Enqueue(client);
                                }
                            }
                        }
                    }
                }
                tot_queue_count1 += q12.Count;
                tot_queue_count2 += q37.Count;
                global_time++;
            }
            //  */
            foreach (Request client in list_ready)
            {
                textBox5.Text += client.ID + " " + client.operation + " " + client.start_time + " " + client.service_time + " " + client.request_state + "\r\n";
            }
            textBox6.Text = tot_reject_XCHG.ToString();
            textBox7.Text = tot_reject_ACNT.ToString();

            avr_queue_count1 = Math.Round(tot_queue_count1 / global_time, 3);
            textBox8.Text = avr_queue_count1.ToString();
            avr_queue_count2 = Math.Round(tot_queue_count2 / global_time, 3);
            textBox9.Text = avr_queue_count2.ToString();
            textBox3.Text = global_time.ToString();
        }
        private int set_service_time(request_type operation, int dec)
        {
            int service_time = 0;
            if (operation == request_type.ACNT)
                service_time = 30 + dec;
            else if (operation == request_type.XCHG)
                service_time = 50 + dec;
            else if (operation == request_type.CARD)
                service_time = 80 + dec;
            else
                service_time = 60 + dec;
            return service_time;
        }
    }
    class Request
    {
        public int ID; //номер заявки
        public int start_time; //время создания заявки
        public int service_time; //время обслуживания заявки
        public int stop_time; //время завершения заявки
        public bool request_state; //состояние заявки
        public readonly request_type operation; //тип операции

        //private Random dec; //отклонение от среднего времени обработки заявки

        public Request(int _ID, int _start_time, int _service_time, bool _request_state, request_type _operation)
        {
            ID = _ID;
            start_time = _start_time;
            service_time = _service_time;
            request_state = _request_state;
            operation = _operation;
        }
    }
    enum kassa_type
    {
        N12, //кассы ХСНG
        N37, //кассы ACNT, CRED, CARD
        N    //  
    }
    enum operator_state //состояние кассы
    {
        Vacant, //свободна
        Occuped, //занята
        NotReady //завершила работу, но еще не освободилась
    }
    class Kassa
    {
        public int ID;
        public kassa_type k_type; //тип кассы
        public operator_state state; //состояние
        private int client_id; //id клиента
        public new Request cl; //клиент
        public Kassa(int _ID, kassa_type _k_type)
        {
            ID = _ID;
            k_type = _k_type;
            state = operator_state.Vacant;
            //cl = new Request();
            cl = null;
        }
    }
}
