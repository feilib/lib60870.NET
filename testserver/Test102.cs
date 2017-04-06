using lib102;
using System;

namespace testserver
{

    public class Test102
    {

        public static void TestFrame()
        {
            TestSinglePoint();
            TestIntegrate();
            TestEndOfInit();
            TestManufacture();
        }

        private static void TestSinglePoint()
        {
            ConnectionParameters para = new ConnectionParameters();
            para.LinkAddress = 1;
            para.SizeOfCA = 2;

            LinkControlUp lc = new LinkControlUp();
            lc.ACD = true;
            lc.DFC = false;
            lc.FuncCode = LinkFunctionCodeUp.UserData;

            T102Frame frame = new T102Frame(lc, para);

            ASDU asdu = new ASDU(TypeID.M_SP_TA_2, CauseOfTransmission.SPONTANEOUS, false, false, 1, RecordAddress.Total, false);
            CP56Time2b tt = new CP56Time2b(new DateTime(2007, 8, 18, 6, 21, 1, 520));

            SinglePointInformation sp = new SinglePointInformation(0, true, 0, tt);
            asdu.AddInformationObject(sp);

            asdu.Encode(frame, para);

            frame.PrepareToSend();

            byte[] aa = frame.GetBuffer();


            int length = aa[1];
            byte linkControl = aa[4];
            int linkAddr = aa[5] + aa[6] * 0x100;

            ASDU na = new ASDU(para, aa, length + 4 + 2);

            InformationObject io = na.GetElement(0);
        }

        private static void TestIntegrate()
        {
            ConnectionParameters para = new ConnectionParameters();
            para.LinkAddress = 1;
            para.SizeOfCA = 2;

            LinkControlUp lc = new LinkControlUp();
            lc.ACD = false;
            lc.DFC = false;
            lc.FuncCode = LinkFunctionCodeUp.UserData;

            T102Frame frame = new T102Frame(lc, para);

            ASDU asdu = new ASDU(CauseOfTransmission.REQUEST, false, false, 1, RecordAddress.Total, false);
            CP40Time2b t = new CP40Time2b(new DateTime(2007, 8, 18, 6, 21, 0));

            OperationalIntegratedTotalsWith3Byte oit = new OperationalIntegratedTotalsWith3Byte(6, 0x030201, false, false, false, 0x12);
            asdu.AddInformationObject(oit);

            asdu.Encode(frame, para);

            //最后编码个时间进去
            frame.AppendBytes(t.GetEncodedValue());

            frame.PrepareToSend();

            byte[] aa = frame.GetBuffer();

            int length = aa[1];
            byte linkControl = aa[4];
            int linkAddr = aa[5] + aa[6] * 0x100;

            ASDU na = new ASDU(para, aa, length + 4 + 2);

            InformationObject io = na.GetElement(0);
            //todo： 没有对最后时间报文的解析内容，还没想到怎么样一个结构去解析之后考虑下。。。
        }

        private static void TestEndOfInit()
        {
            ConnectionParameters para = new ConnectionParameters();
            para.LinkAddress = 1;
            para.SizeOfCA = 2;

            LinkControlUp lc = new LinkControlUp();
            lc.ACD = false;
            lc.DFC = false;
            lc.FuncCode = LinkFunctionCodeUp.UserData;

            T102Frame frame = new T102Frame(lc, para);

            ASDU asdu = new ASDU(CauseOfTransmission.INITIALIZED, false, false, 1, RecordAddress.Default, false);
            EndOfInit eoi = new EndOfInit(CauseOfInit.LocalPowerOn, false);
            asdu.AddInformationObject(eoi);

            asdu.Encode(frame, para);

            frame.PrepareToSend();

            byte[] aa = frame.GetBuffer();

            int length = aa[1];
            byte linkControl = aa[4];
            int linkAddr = aa[5] + aa[6] * 0x100;

            ASDU na = new ASDU(para, aa, length + 4 + 2);

            InformationObject io = na.GetElement(0);
        }

        private static void TestManufacture()
        {
            ConnectionParameters para = new ConnectionParameters();
            para.LinkAddress = 1;
            para.SizeOfCA = 2;

            LinkControlUp lc = new LinkControlUp();
            lc.ACD = false;
            lc.DFC = false;
            lc.FuncCode = LinkFunctionCodeUp.UserData;

            T102Frame frame = new T102Frame(lc, para);

            ASDU asdu = new ASDU(CauseOfTransmission.INITIALIZED, false, false, 1, RecordAddress.Default, false);

            ManufacturerSpec ms = new ManufacturerSpec(0, 1, 0, 0x05040302);
            asdu.AddInformationObject(ms);

            asdu.Encode(frame, para);

            frame.PrepareToSend();

            byte[] aa = frame.GetBuffer();

            int length = aa[1];
            byte linkControl = aa[4];
            int linkAddr = aa[5] + aa[6] * 0x100;

            ASDU na = new ASDU(para, aa, length + 4 + 2);

            InformationObject io = na.GetElement(0);
        }
    }
}
