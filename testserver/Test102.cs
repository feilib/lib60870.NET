using lib102;
using System;

namespace testserver
{

    public class Test102
    {

        public static void TestFrame()
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

            byte[] aa= frame.GetBuffer();


            int length = aa[1];
            byte linkControl = aa[4];
            int linkAddr = aa[5]+aa[6]*0x100;
           
            ASDU na = new ASDU(para, aa, length+4+2);

            InformationObject io = na.GetElement(1);

        }
       
    }
}
