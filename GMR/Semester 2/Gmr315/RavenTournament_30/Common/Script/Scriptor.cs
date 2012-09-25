using System;
using System.IO;
using System.Text;
using LuaInterface;

namespace Common.Script
{
    public class Scriptor
    {
        public static void RunScriptFile(Lua lua, string ScriptName)
        {
            // LUA is particular about encoding type, so we 
            // always open and resave file in ASCII regardless
            // of previous encoding.
            string script = File.ReadAllText(ScriptName);
            string temporaryFilename = FileUtils.GetTemporaryFile("lua");
            File.WriteAllText(temporaryFilename, script, Encoding.ASCII);
            lua.DoFile(temporaryFilename);
        }

        private readonly Lua m_pLuaState;

        public
            Scriptor()
        {
            m_pLuaState = new Lua();
        }

        ~Scriptor()
        {
            m_pLuaState.Close();
        }

        public void RunScriptFile(string ScriptName)
        {
            // LUA is particular about encoding type, so we 
            // always open and resave file in ASCII regardless
            // of previous encoding.
            string script = File.ReadAllText(ScriptName);
            string temporaryFilename = FileUtils.GetTemporaryFile("lua");
            File.WriteAllText(temporaryFilename, script, Encoding.ASCII);
            m_pLuaState.DoFile(temporaryFilename);
        }

        public Lua GetState()
        {
            return m_pLuaState;
        }


        public int GetInt(string VariableName)
        {
            try
            {
                return (int) m_pLuaState.GetNumber(VariableName);
            }
            catch (Exception e)
            {
                throw new Exception(
                    "Error in GetInt.  Most likely cause Variable: " + VariableName + " does not exist", e);
            }
        }

        public float GetFloat(string VariableName)
        {
            try {
            return (float) m_pLuaState.GetNumber(VariableName);
            }
            catch (Exception e)
            {
                throw new Exception(
                    "Error in GetFloat.  Most likely cause Variable: " + VariableName + " does not exist", e);
            }
        }

        public float GetDouble(string VariableName)
        {
            try
            {
                return (float) m_pLuaState.GetNumber(VariableName);
            } catch (Exception e)
            {
                throw new Exception(
                    "Error in GetDouble.  Most likely cause Variable: " + VariableName + " does not exist", e);
            }
            
        }

        public string GetString(string VariableName)
        {
            try {
            return m_pLuaState.GetString(VariableName);
            }
            catch (Exception e)
            {
                throw new Exception(
                    "Error in GetString.  Most likely cause Variable: " + VariableName + " does not exist", e);
            }
        }

        public bool GetBool(string VariableName)
        {
            try {
            string x = m_pLuaState.GetString(VariableName);
            return bool.Parse(x);
            }
            catch (Exception e)
            {
                throw new Exception(
                    "Error in GetBool.  Most likely cause Variable: " + VariableName + " does not exist", e);
            }
        }
    }
}