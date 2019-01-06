using Ambush.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ambush.Enums;

namespace Ambush.Components
{
    public class Component 
    {
        protected int id { get; set; }
        public State state { get; set; }
        public string source { get; set; }

        public int CPXId;

        public Component(int id,int CPXId)
        {
            this.id = id;
            this.state = State.OFF;
            this.CPXId = CPXId;
        }
        

        public void setRequest(State newState)
        {
            //Build a string in the correct form of a SET request

            
            StringBuilder builder = new StringBuilder();
            builder.Append("AR:");
            builder.Append("SET:");
            builder.Append(source);
            builder.Append(":");
            builder.Append(id.ToString());
            builder.Append(":");
            builder.Append(newState.ToString());
            using (TCPClient client = new TCPClient(builder.ToString(),CPXId)) { }
            
        }
        public void getRequest()
        {
            //Build a string in the correct form of a GET request

            StringBuilder builder = new StringBuilder();
            builder.Append("AR:");
            builder.Append("GET:");
            builder.Append(source);
            builder.Append(":");
            builder.Append(id.ToString());
            builder.Append(":");
            using (TCPClient client = new TCPClient(builder.ToString(), CPXId)) { }

        }


        protected int getId()
        {
            return id;
        }



    }
}
