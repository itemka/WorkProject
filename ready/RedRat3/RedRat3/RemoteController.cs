using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedRat3
{
    public partial class RemoteController : Form
    {
        public RemoteController()
        {
            InitializeComponent();
        }


        private void button4_Click(object sender, EventArgs e)
        {//Включение/Выключение

        }
        private void button4_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button4, "Включение/Выключение");
        }


        private void button5_Click(object sender, EventArgs e)
        {//Без звука

        }
        private void button5_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button5, "Без звука");
        }


        private void button6_Click(object sender, EventArgs e)
        {//Красная кнопка

        }
        private void button6_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button6, "Красная кнопка");
        }


        private void button7_Click(object sender, EventArgs e)
        {//Зеленая кнопка

        }
        private void button7_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button7, "Зеленая кнопка");
        }


        private void button8_Click(object sender, EventArgs e)
        {//Желтая кнопка

        }
        private void button8_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button8, "Желтая кнопка");
        }


        private void button9_Click(object sender, EventArgs e)
        {//Синяя кнопка

        }
        private void button9_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button9, "Синяя кнопка");
        }


        private void button10_Click(object sender, EventArgs e)
        {//Перемотка назад

        }
        private void button10_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button10, "Перемотка назад");
        }


        private void button11_Click(object sender, EventArgs e)
        {//Перемотка вперед

        }
        private void button11_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button11, "Перемотка вперед");
        }


        private void button12_Click(object sender, EventArgs e)
        {//Перемотка в самое начало

        }
        private void button12_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button12, "Перемотка в самое начало");
        }


        private void button13_Click(object sender, EventArgs e)
        {//Перемотка в самый конец

        }
        private void button13_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button13, "Перемотка в самый конец");
        }


        private void button14_Click(object sender, EventArgs e)
        {//Пуск/Пауза

        }
        private void button14_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button14, "Пуск/Пауза");
        }


        private void button15_Click(object sender, EventArgs e)
        {//Стоп

        }
        private void button15_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button15, "Стоп");
        }


        private void button16_Click(object sender, EventArgs e)
        {//FAV.

        }
        private void button16_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button16, "FAV.");
        }


        private void button17_Click(object sender, EventArgs e)
        {//SCALE

        }
        private void button17_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button17, "SCALE");
        }


        private void button18_Click(object sender, EventArgs e)
        {//EPG

        }
        private void button18_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button18, "EPG");
        }


        private void button19_Click(object sender, EventArgs e)
        {//DTV/RADIO

        }
        private void button19_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button19, "DTV/RADIO");
        }


        private void button20_Click(object sender, EventArgs e)
        {//REG

        }
        private void button20_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button20, "REG");
        }


        private void button21_Click(object sender, EventArgs e)
        {//SYBTITLE

        }
        private void button21_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button21, "SYBTITLE");
        }


        private void button22_Click(object sender, EventArgs e)
        {//P.MODE

        }
        private void button22_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button22, "P.MODE");
        }


        private void button23_Click(object sender, EventArgs e)
        {//S.MODE

        }
        private void button23_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button23, "S.MODE");
        }


        private void button24_Click(object sender, EventArgs e)
        {//SLEEP

        }
        private void button24_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button24, "SLEEP");
        }


        private void button25_Click(object sender, EventArgs e)
        {//AUDIO

        }
        private void button25_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button25, "AUDIO");
        }


        private void button26_Click(object sender, EventArgs e)
        {//MENU

        }
        private void button26_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button26, "MENU");
        }


        private void button27_Click(object sender, EventArgs e)
        {//SOURCE

        }
        private void button27_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button27, "SOURCE");
        }


        private void button33_Click(object sender, EventArgs e)
        {//EXIT

        }
        private void button33_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button33, "EXIT");
        }


        private void button34_Click(object sender, EventArgs e)
        {//INFO

        }
        private void button34_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button34, "INFO");
        }


        private void button28_Click(object sender, EventArgs e)
        {//ВВЕРХ

        }
        private void button28_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button28, "ВВЕРХ");
        }


        private void button30_Click(object sender, EventArgs e)
        {//ВНИЗ

        }
        private void button30_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button30, "ВНИЗ");
        }


        private void button29_Click(object sender, EventArgs e)
        {//ВЛЕВО

        }
        private void button29_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button29, "ВЛЕВО");
        }


        private void button31_Click(object sender, EventArgs e)
        {//ВПРАВО

        }
        private void button31_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button31, "ВПРАВО");
        }


        private void button32_Click(object sender, EventArgs e)
        {//ENTER/OK

        }
        private void button32_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button32, "ENTER/OK");
        }


        private void button35_Click(object sender, EventArgs e)
        {//CH+

        }
        private void button35_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button35, "CH+");
        }


        private void button36_Click(object sender, EventArgs e)
        {//CH-

        }
        private void button36_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button36, "CH-");
        }


        private void button39_Click(object sender, EventArgs e)
        {//HOME

        }
        private void button39_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button39, "HOME");
        }


        private void button40_Click(object sender, EventArgs e)
        {//MOUSE

        }
        private void button40_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button40, "MOUSE");
        }


        private void button37_Click(object sender, EventArgs e)
        {//VOL+

        }
        private void button37_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button37, "VOL+");
        }


        private void button38_Click(object sender, EventArgs e)
        {//VOL-

        }
        private void button38_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button38, "VOL-");
        }


        private void button41_Click(object sender, EventArgs e)
        {//1

        }
        private void button41_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button41, "1");
        }


        private void button42_Click(object sender, EventArgs e)
        {//2

        }
        private void button42_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button42, "2");
        }


        private void button43_Click(object sender, EventArgs e)
        {//3

        }
        private void button43_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button43, "3");
        }


        private void button44_Click(object sender, EventArgs e)
        {//4

        }
        private void button44_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button44, "4");
        }


        private void button45_Click(object sender, EventArgs e)
        {//5

        }
        private void button45_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button45, "5");
        }


        private void button46_Click(object sender, EventArgs e)
        {//6

        }
        private void button46_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button46, "6");
        }


        private void button47_Click(object sender, EventArgs e)
        {//7

        }
        private void button47_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button47, "7");
        }


        private void button48_Click(object sender, EventArgs e)
        {//8

        }
        private void button48_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button48, "8");
        }


        private void button49_Click(object sender, EventArgs e)
        {//9

        }
        private void button49_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button49, "9");
        }


        private void button50_Click(object sender, EventArgs e)
        {//0

        }
        private void button50_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button50, "0");
        }


        private void button51_Click(object sender, EventArgs e)
        {//STILL

        }
        private void button51_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button51, "STILL");
        }


        private void button52_Click(object sender, EventArgs e)
        {//RESET

        }
        private void button52_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button52, "RESET");
        }


        private void button53_Click(object sender, EventArgs e)
        {//FILE

        }
        private void button53_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button53, "FILE");
        }


        private void button54_Click(object sender, EventArgs e)
        {//ZOOM-

        }
        private void button54_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button54, "ZOOM-");
        }


        private void button55_Click(object sender, EventArgs e)
        {//ZOOM+

        }
        private void button55_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button55, "ZOOM+");
        }








    }
}
