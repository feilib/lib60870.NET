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
            TestCurrentTime();

            TestReadMnufacture();
            TestReadSinglePoint();
            TestReadSinglePointWithRange();

            TestReadCurrentTime();


            TestReadAccountingIT();
            TestReadAccountingITWithAddressRange();
            TestReadAccountingITWithSpecificTime();
            TestReadAccountingITWithSpecificTimeAndAddressRange();
            TestReadAccountingITWithTimeRangeAndAddressRange();
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

        private static void TestCurrentTime()
        {
            ConnectionParameters para = new ConnectionParameters();
            para.LinkAddress = 1;
            para.SizeOfCA = 2;

            LinkControlUp lc = new LinkControlUp();
            lc.ACD = false;
            lc.DFC = false;
            lc.FuncCode = LinkFunctionCodeUp.UserData;

            T102Frame frame = new T102Frame(lc, para);

            ASDU asdu = new ASDU(CauseOfTransmission.REQUEST, false, false, 1, RecordAddress.Default, false);

            CurrentTime ct = new CurrentTime(new CP56Time2b(new DateTime(2007, 8, 18, 6, 21, 1, 520)));
            asdu.AddInformationObject(ct);

            asdu.Encode(frame, para);

            frame.PrepareToSend();

            byte[] aa = frame.GetBuffer();

            int length = aa[1];
            byte linkControl = aa[4];
            int linkAddr = aa[5] + aa[6] * 0x100;

            ASDU na = new ASDU(para, aa, length + 4 + 2);

            InformationObject io = na.GetElement(0);


        }

        private static void TestReadMnufacture()
        {
            ConnectionParameters para = new ConnectionParameters();
            para.LinkAddress = 1;
            para.SizeOfCA = 2;

            LinkControlDown lc = new LinkControlDown();
            lc.FCB = false;
            lc.FCV = true;
            lc.FuncCode = LinkFunctionCodeDown.UserData;

            T102Frame frame = new T102Frame(lc, para);

            //没有信息体，直接做个ASDU就好了。。。
            ASDU asdu = new ASDU(TypeID.C_RD_NA_2, CauseOfTransmission.REQUEST, false, false, 1, RecordAddress.Default, false);

            asdu.Encode(frame, para);

            frame.PrepareToSend();

            byte[] aa = frame.GetBuffer();

            int length = aa[1];
            byte linkControl = aa[4];
            int linkAddr = aa[5] + aa[6] * 0x100;

            //解析
            ASDU na = new ASDU(para, aa, length + 4 + 2);
            //无数据
            na.Cot = CauseOfTransmission.NO_RECORD;

            LinkControlUp lc2 = new LinkControlUp();
            lc2.ACD = false;
            lc2.DFC = false;
            lc2.FuncCode = LinkFunctionCodeUp.NoData;

            T102Frame frame2 = new T102Frame(lc2, para);

            na.Encode(frame2, para);
            frame2.PrepareToSend();
            //镜像报文
            byte[] bb = frame2.GetBuffer();
        }

        private static void TestReadSinglePoint()
        {
            ConnectionParameters para = new ConnectionParameters();
            para.LinkAddress = 1;
            para.SizeOfCA = 2;

            LinkControlDown lc = new LinkControlDown();
            lc.FCB = false;
            lc.FCV = true;
            lc.FuncCode = LinkFunctionCodeDown.UserData;

            T102Frame frame = new T102Frame(lc, para);

            //没有信息体，直接做个ASDU就好了。。。
            ASDU asdu = new ASDU(TypeID.C_SP_NA_2, CauseOfTransmission.ACTIVATION, false, false, 1, RecordAddress.Default, false);

            asdu.Encode(frame, para);

            frame.PrepareToSend();

            byte[] aa = frame.GetBuffer();

            int length = aa[1];
            byte linkControl = aa[4];
            int linkAddr = aa[5] + aa[6] * 0x100;

            //解析
            ASDU na = new ASDU(para, aa, length + 4 + 2);
            //无数据
            na.Cot = CauseOfTransmission.NO_RECORD;

            LinkControlUp lc2 = new LinkControlUp();
            lc2.ACD = false;
            lc2.DFC = false;
            lc2.FuncCode = LinkFunctionCodeUp.NoData;

            T102Frame frame2 = new T102Frame(lc2, para);

            na.Encode(frame2, para);
            frame2.PrepareToSend();
            //镜像报文
            byte[] bb = frame2.GetBuffer();
        }

        private static void TestReadSinglePointWithRange()
        {
            ConnectionParameters para = new ConnectionParameters();
            para.LinkAddress = 1;
            para.SizeOfCA = 2;

            LinkControlDown lc = new LinkControlDown();
            lc.FCB = false;
            lc.FCV = true;
            lc.FuncCode = LinkFunctionCodeDown.UserData;

            T102Frame frame = new T102Frame(lc, para);

            ASDU asdu = new ASDU(CauseOfTransmission.ACTIVATION, false, false, 1, RecordAddress.Period_Week_1, false);
            ReadSinglePointWithTimeRange sptr = new ReadSinglePointWithTimeRange(
                                                new CP40Time2b(new DateTime(2007, 9, 2, 1, 0, 0)),
                                                new CP40Time2b(new DateTime(2007, 9, 3, 1, 0, 0)));
            asdu.AddInformationObject(sptr);
            asdu.Encode(frame, para);

            frame.PrepareToSend();

            //todo: 记录类型书上表的是0x31，这里测试结果是十进制31，根据国外标准，31应该是十进制。具体情况具体分析把。。 
            byte[] aa = frame.GetBuffer();


            int length = aa[1];
            byte linkControl = aa[4];
            int linkAddr = aa[5] + aa[6] * 0x100;

            //解析
            ASDU na = new ASDU(para, aa, length + 4 + 2);
            InformationObject sp = na.GetElement(0);
            //无数据
            na.Cot = CauseOfTransmission.NO_RECORD;

            LinkControlUp lc2 = new LinkControlUp();
            lc2.ACD = false;
            lc2.DFC = false;
            lc2.FuncCode = LinkFunctionCodeUp.NoData;

            T102Frame frame2 = new T102Frame(lc2, para);

            na.Encode(frame2, para);
            frame2.PrepareToSend();
            //镜像报文
            byte[] bb = frame2.GetBuffer();
        }


        private static void TestReadCurrentTime()
        {
            ConnectionParameters para = new ConnectionParameters();
            para.LinkAddress = 1;
            para.SizeOfCA = 2;

            LinkControlDown lc = new LinkControlDown();
            lc.FCB = true;
            lc.FCV = true;
            lc.FuncCode = LinkFunctionCodeDown.UserData;

            T102Frame frame = new T102Frame(lc, para);

            ASDU asdu = new ASDU(TypeID.C_TI_NA_2, CauseOfTransmission.REQUEST, false, false, 1, RecordAddress.Default, false);

            asdu.Encode(frame, para);

            frame.PrepareToSend();


            byte[] aa = frame.GetBuffer();


            int length = aa[1];
            byte linkControl = aa[4];
            int linkAddr = aa[5] + aa[6] * 0x100;

            //解析
            ASDU na = new ASDU(para, aa, length + 4 + 2);
            //无ASDU
            na.Cot = CauseOfTransmission.NO_ASDU_TYPE;

            LinkControlUp lc2 = new LinkControlUp();
            lc2.ACD = false;
            lc2.DFC = false;
            lc2.FuncCode = LinkFunctionCodeUp.NoData;

            T102Frame frame2 = new T102Frame(lc2, para);

            na.Encode(frame2, para);
            frame2.PrepareToSend();
            //镜像报文
            byte[] bb = frame2.GetBuffer();
        }

        private static void TestReadAccountingIT()
        {
            ConnectionParameters para = new ConnectionParameters();
            para.LinkAddress = 1;
            para.SizeOfCA = 2;

            LinkControlDown lc = new LinkControlDown();
            lc.FCB = false;
            lc.FCV = true;
            lc.FuncCode = LinkFunctionCodeDown.UserData;

            T102Frame frame = new T102Frame(lc, para);

            //没有信息体，直接做个ASDU就好了。。。
            ASDU asdu = new ASDU(TypeID.C_CI_NA_2, CauseOfTransmission.ACTIVATION, false, false, 1, RecordAddress.Default, false);

            asdu.Encode(frame, para);

            frame.PrepareToSend();

            byte[] aa = frame.GetBuffer();

            int length = aa[1];
            byte linkControl = aa[4];
            int linkAddr = aa[5] + aa[6] * 0x100;

            //解析
            ASDU na = new ASDU(para, aa, length + 4 + 2);
            //无数据
            na.Cot = CauseOfTransmission.NO_RECORD;

            LinkControlUp lc2 = new LinkControlUp();
            lc2.ACD = false;
            lc2.DFC = false;
            lc2.FuncCode = LinkFunctionCodeUp.NoData;

            T102Frame frame2 = new T102Frame(lc2, para);

            na.Encode(frame2, para);
            frame2.PrepareToSend();
            //镜像报文
            byte[] bb = frame2.GetBuffer();
        }

        private static void TestReadAccountingITWithAddressRange()
        {
            ConnectionParameters para = new ConnectionParameters();
            para.LinkAddress = 1;
            para.SizeOfCA = 2;

            LinkControlDown lc = new LinkControlDown();
            lc.FCB = false;
            lc.FCV = true;
            lc.FuncCode = LinkFunctionCodeDown.UserData;

            T102Frame frame = new T102Frame(lc, para);

            ASDU asdu = new ASDU(CauseOfTransmission.ACTIVATION, false, false, 1, RecordAddress.Default, false);
            ReadAccountingITWithAddressRange ar = new lib102.ReadAccountingITWithAddressRange(3, 7);
            asdu.AddInformationObject(ar);
            asdu.Encode(frame, para);

            frame.PrepareToSend();

            //todo 书上实例报文，记录地址是03，这个有点扯。。。
            byte[] aa = frame.GetBuffer();


            int length = aa[1];
            byte linkControl = aa[4];
            int linkAddr = aa[5] + aa[6] * 0x100;

            //解析
            ASDU na = new ASDU(para, aa, length + 4 + 2);
            InformationObject sp = na.GetElement(0);
            //无数据
            na.Cot = CauseOfTransmission.NO_RECORD;

            LinkControlUp lc2 = new LinkControlUp();
            lc2.ACD = false;
            lc2.DFC = false;
            lc2.FuncCode = LinkFunctionCodeUp.NoData;

            T102Frame frame2 = new T102Frame(lc2, para);

            na.Encode(frame2, para);
            frame2.PrepareToSend();
            //镜像报文
            byte[] bb = frame2.GetBuffer();
        }

        private static void TestReadAccountingITWithSpecificTime()
        {
            ConnectionParameters para = new ConnectionParameters();
            para.LinkAddress = 1;
            para.SizeOfCA = 2;

            LinkControlDown lc = new LinkControlDown();
            lc.FCB = true;
            lc.FCV = true;
            lc.FuncCode = LinkFunctionCodeDown.UserData;

            T102Frame frame = new T102Frame(lc, para);

            ASDU asdu = new ASDU(CauseOfTransmission.ACTIVATION, false, false, 1, RecordAddress.Default, false);
            ReadAccountingITWithSpecificTime at = new ReadAccountingITWithSpecificTime(
                                                        new CP40Time2b(new DateTime(2007, 8, 18, 6, 21, 0)));
            asdu.AddInformationObject(at);
            asdu.Encode(frame, para);

            frame.PrepareToSend();

            //todo 书上实例报文，记录地址是03，这个有点扯。。。
            byte[] aa = frame.GetBuffer();


            int length = aa[1];
            byte linkControl = aa[4];
            int linkAddr = aa[5] + aa[6] * 0x100;

            //解析
            ASDU na = new ASDU(para, aa, length + 4 + 2);
            InformationObject sp = na.GetElement(0);
            //无数据
            na.Cot = CauseOfTransmission.NO_RECORD;

            LinkControlUp lc2 = new LinkControlUp();
            lc2.ACD = false;
            lc2.DFC = false;
            lc2.FuncCode = LinkFunctionCodeUp.NoData;

            T102Frame frame2 = new T102Frame(lc2, para);

            na.Encode(frame2, para);
            frame2.PrepareToSend();
            //镜像报文
            byte[] bb = frame2.GetBuffer();
        }

        private static void TestReadAccountingITWithSpecificTimeAndAddressRange()
        {
            ConnectionParameters para = new ConnectionParameters();
            para.LinkAddress = 1;
            para.SizeOfCA = 2;

            LinkControlDown lc = new LinkControlDown();
            lc.FCB = true;
            lc.FCV = true;
            lc.FuncCode = LinkFunctionCodeDown.UserData;

            T102Frame frame = new T102Frame(lc, para);

            ASDU asdu = new ASDU(CauseOfTransmission.ACTIVATION, false, false, 1, RecordAddress.Default, false);
            ReadAccountingITWithSpecificTimeAndAddressRange at = new ReadAccountingITWithSpecificTimeAndAddressRange(
                                                    3, 7, new CP40Time2b(new DateTime(2007, 8, 18, 6, 21, 0)));

            asdu.AddInformationObject(at);
            asdu.Encode(frame, para);

            frame.PrepareToSend();

            //todo 书上实例报文，记录地址是03，这个有点扯。。。
            byte[] aa = frame.GetBuffer();


            int length = aa[1];
            byte linkControl = aa[4];
            int linkAddr = aa[5] + aa[6] * 0x100;

            //解析
            ASDU na = new ASDU(para, aa, length + 4 + 2);
            InformationObject sp = na.GetElement(0);
            //无数据
            na.Cot = CauseOfTransmission.NO_RECORD;

            LinkControlUp lc2 = new LinkControlUp();
            lc2.ACD = false;
            lc2.DFC = false;
            lc2.FuncCode = LinkFunctionCodeUp.NoData;

            T102Frame frame2 = new T102Frame(lc2, para);

            na.Encode(frame2, para);
            frame2.PrepareToSend();
            //镜像报文
            byte[] bb = frame2.GetBuffer();
        }

        private static void TestReadAccountingITWithTimeRangeAndAddressRange()
        {
            ConnectionParameters para = new ConnectionParameters();
            para.LinkAddress = 1;
            para.SizeOfCA = 2;

            LinkControlDown lc = new LinkControlDown();
            lc.FCB = false;
            lc.FCV = true;
            lc.FuncCode = LinkFunctionCodeDown.UserData;

            T102Frame frame = new T102Frame(lc, para);

            ASDU asdu = new ASDU(CauseOfTransmission.ACTIVATION, false, false, 1, RecordAddress.Default, false);
            ReadAccountingITWithTimeRangeAndAddressRange at = new ReadAccountingITWithTimeRangeAndAddressRange(
                                                        3, 7, new CP40Time2b(new DateTime(2007, 8, 18, 0, 0, 0)),
                                                        new CP40Time2b(new DateTime(2007, 8, 19, 0, 0, 0)));
            asdu.AddInformationObject(at);
            asdu.Encode(frame, para);

            frame.PrepareToSend();

            //todo 书上实例报文，记录地址是03，这个有点扯。。。
            byte[] aa = frame.GetBuffer();


            int length = aa[1];
            byte linkControl = aa[4];
            int linkAddr = aa[5] + aa[6] * 0x100;

            //解析
            ASDU na = new ASDU(para, aa, length + 4 + 2);
            InformationObject sp = na.GetElement(0);
            //无数据
            na.Cot = CauseOfTransmission.NO_RECORD;

            LinkControlUp lc2 = new LinkControlUp();
            lc2.ACD = false;
            lc2.DFC = false;
            lc2.FuncCode = LinkFunctionCodeUp.NoData;

            T102Frame frame2 = new T102Frame(lc2, para);

            na.Encode(frame2, para);
            frame2.PrepareToSend();
            //镜像报文
            byte[] bb = frame2.GetBuffer();
        }

    }
}
