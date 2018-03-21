using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using MSMQLib;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            send();
            //receive();
            //receiveSimpleMsg();
            Console.Read();
        }

        public static void send()
        {
            MessageQueue MQ = new MessageQueue(".\\private$\\TestQueue");
            //MQ.Send("消息测试", "测试消息");
            OrderStruct model = new OrderStruct();
            model.NickName = "你好呀阿道夫上的";
            model.SupplierName = "哈哈";
            //MQ.Send(model);
            Message message = new Message();
            message.Label = "消息lable";
            message.Body = model;
            //MQ.Send(message);
            MSMQManager.Instance.Send(model);
            Console.WriteLine("结束");
        }
        protected static void sendSimpleMsg()
        {
            //实例化MessageQueue,并指向现有的一个名称为VideoQueue队列  
            MessageQueue MQ = new MessageQueue(@".\private$\MsgQueue");
            //MQ.Send("消息测试", "测试消息");  
            System.Messaging.Message message = new System.Messaging.Message();
            message.Label = "消息lable";
            message.Body = "消息body";
            MQ.Send(message);

            Console.Write("成功发送消息，" + DateTime.Now + "<br/>");
        }
        protected static void receiveSimpleMsg()
        {
            MessageQueue MQ = new MessageQueue(@".\private$\ordertest");
            //调用MessageQueue的Receive方法接收消息  
            if (MQ.GetAllMessages().Length > 0)
            {
                MQ.Formatter = new XmlMessageFormatter(new Type[] { typeof(ConsoleApplication1.OrderStruct) });
                System.Messaging.Message message = MQ.Receive();
                if (message != null)
                {
                    //message.Formatter = new System.Messaging.XmlMessageFormatter(new string[] { "Message.Bussiness.VideoPath,Message" });//消息类型转换  

                    OrderStruct data = (OrderStruct)message.Body;
                    //Console.WriteLine(string.Format("接收消息成功,lable:{0},body:{1},{2}<br/>", message.Label, data.NickName, DateTime.Now));
                    Console.WriteLine("商品名称" + data.NickName);
                }
            }
            else
            {
                Console.WriteLine("没有消息了！<br/>");
            }
        }

        public static void receive()
        {
            var instance = MSMQManager.Instance;
            instance.AsyncPeek((o, e) =>
                {
                    System.Messaging.Message message = null;
                    MessageQueue messageQueue = null;

                    var messageQueueTransaction = new MessageQueueTransaction();
                    try
                    {
                        //MessageQueue MQ = new MessageQueue(@".\private$\TestQueue");

                        messageQueue = (MessageQueue)o;
                        messageQueue.Formatter = new BinaryMessageFormatter();
                        //messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(ConsoleApplication1.OrderStruct) });
                        messageQueue.EndPeek(e.AsyncResult);

                        messageQueueTransaction.Begin();
                        message = messageQueue.Receive(messageQueueTransaction);
                        //调用MessageQueue的Receive方法接收消息  
                        //if (MQ.GetAllMessages().Length > 0)
                        //{
                        //MQ.Formatter = new BinaryMessageFormatter();
                        //System.Messaging.Message message = MQ.Receive();
                        if (message != null)
                        {
                            //message.Formatter = new System.Messaging.XmlMessageFormatter(new string[] { "Message.Bussiness.VideoPath,Message" });//消息类型转换

                            //message.Formatter = new ActiveXMessageFormatter();
                            //var data = message.Body as OrderStruct;
                            //message.Formatter = new System.Messaging.XmlMessageFormatter(new Type[] { typeof(OrderStruct) });//消息类型转换
                            var msg = message.Body as OrderStruct;
                            if (msg != null)
                            {
                                Console.WriteLine("商品名称" + msg.NickName);
                            }
                            //Console.WriteLine(string.Format("接收消息成功,lable:{0},body:{1},{2}<br/>", message.Label, data, DateTime.Now));
                        }
                        messageQueueTransaction.Commit();
                        //}
                        //else
                        //{
                        //    Console.WriteLine("没有消息了！<br/>");
                        //}
                    }
                    catch (Exception ex)
                    {
                        messageQueueTransaction.Abort();
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        if (messageQueue != null)
                        {
                            messageQueue.BeginPeek();
                        }
                    }
                });
        }
    }
}
