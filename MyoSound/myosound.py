# -*- coding: utf-8 -*-
#------------------------------------------------------------------------------
# FEDERAL UNIVERSITY OF UBERLANDIA
# Faculty of Electrical Engineering
# Biomedical Engineering Lab
#------------------------------------------------------------------------------
# Author: Italo Gustavo Sampaio Fernandes
# Contact: italogsfernandes@gmail.com
# Git: www.github.com/italogfernandes
#------------------------------------------------------------------------------
# Decription:
#------------------------------------------------------------------------------
import webbrowser
from PyQt4.QtCore import *
from PyQt4.QtGui import *
from PyQt4.uic import loadUiType
from threading import Thread, Lock, Timer
from Queue import Queue
from datetime import datetime
from sets import Set
import numpy as np
from threadhandler import ThreadHandler
from arduinohandler import *
import math
import serial.tools.list_ports as serial_tools
from datetime import datetime
from mingus.midi import  fluidsynth
from mingus.containers import Note
from mingus.containers import MidiInstrument

#------------------------------------------------------------------------------
Ui_MainWindow, QMainWindow = loadUiType('mainwindow.ui')
#------------------------------------------------------------------------------
class Main(QMainWindow, Ui_MainWindow):
	def __init__(self):
		super(Main,self).__init__()
		self.setupUi(self)

		self.avr  = None
		self.thresholdCh0 = 1.0
		self.thresholdCh1 = 1.0
		self.ch0Contraido = False
		self.ch1Contraido = False

		self.stateChanged = False
		self.ch0State = False
		self.ch1State = False


		fluidsynth.init('FluidR3_GM.sf2','alsa')
		self.soundCommand1 = Note('C', 4)
		self.soundCommand2 = Note('E', 4)
		self.soundCommand3 = Note('G', 4)
		self.ihmMIDIinstrument = MidiInstrument()

		self.populateSerialPorts()
		self.populateInstruments()
		self.populateNotesAndScales()

		self.btnConnectDisconnect.clicked.connect(self.doConnect)
		self.btnStartStop.clicked.connect(self.doStartStop)
		self.sliderCh0.valueChanged.connect(self.emgValueChanged)
		self.sliderCh1.valueChanged.connect(self.emgValueChanged)
		self.sliderThCh0.valueChanged.connect(self.thresholdChanged)
		self.sliderThCh1.valueChanged.connect(self.thresholdChanged)
		self.connect(self.cbInstruments,SIGNAL('currentIndexChanged(int)'),self.cbInstrumentsChanged)
		self.connect(self.cbNota1,SIGNAL('currentIndexChanged(int)'),self.cbNotasChanged)
		self.connect(self.cbNota2,SIGNAL('currentIndexChanged(int)'),self.cbNotasChanged)
		self.connect(self.cbNota3,SIGNAL('currentIndexChanged(int)'),self.cbNotasChanged)
		self.connect(self.cbEscala1,SIGNAL('currentIndexChanged(int)'),self.cbNotasChanged)
		self.connect(self.cbEscala2,SIGNAL('currentIndexChanged(int)'),self.cbNotasChanged)
		self.connect(self.cbEscala3,SIGNAL('currentIndexChanged(int)'),self.cbNotasChanged)

		self.scene = QGraphicsScene(self)
		self.graphResult.setScene(self.scene)
		self.normalColor = QBrush(QColor.fromRgb(162,178,245))
		self.hilightColor = QBrush(QColor.fromRgb(57,255,77))
		self.outlinePen = QPen(Qt.black)
		self.outlinePen.setWidth(1)

		self.circleSilence = self.scene.addEllipse(-100,-100,100,100,self.outlinePen,self.hilightColor)
		self.circleNota2 = self.scene.addEllipse(-100,100,100,100,self.outlinePen,self.normalColor)
		self.circleNota1 = self.scene.addEllipse(100,-100,100,100,self.outlinePen,self.normalColor)
		self.circleNota3 = self.scene.addEllipse(100,100,100,100,self.outlinePen,self.normalColor)
		self.textSilence = self.scene.addText("Silencio", QFont("Arial", 12))
		self.textNota1 = self.scene.addText("Nota 1", QFont("Arial", 12))
		self.textNota2 = self.scene.addText("Nota 2", QFont("Arial", 12))
		self.textNota3 = self.scene.addText("Nota 3", QFont("Arial", 12))
		self.textSilence.setPos(-80,-62.5)
		self.textNota2.setPos(-75,138.5)
		self.textNota1.setPos(125,-62.5)
		self.textNota3.setPos(125,138.5)


	def show_error_msg(self,msg_to_show):
		msg = QMessageBox()
		msg.setIcon(QMessageBox.Warning)
		msg.setText(msg_to_show)
		msg.setWindowTitle("Erro")
		retval = msg.exec_()

	def show_info_msg(self,msg_to_show):
		msg = QMessageBox()
		msg.setIcon(QMessageBox.Information)
		msg.setText(msg_to_show)
		msg.setWindowTitle("Mensagem Info")
		retval = msg.exec_()

	def populateSerialPorts(self):
		for serial_port in serial_tools.comports():
			self.cbSerialPorts.addItem(serial_port.device)

		if len(serial_tools.comports()) == 0:
			self.show_error_msg("Nenhuma porta Serial Disponivel")
			self.cbSerialPorts.setEnabled(False)
			self.btnStartStop.setEnabled(False)
		else:
			self.cbSerialPorts.setCurrentIndex(len(serial_tools.comports())-1)

	def doConnect(self):
		if self.avr == None:
			self.avr = Arduino(self.cbSerialPorts.itemText(self.cbSerialPorts.currentIndex()))

		if self.avr != None:
			if self.avr.serialPort == None:
				try:
					if self.avr.open():
						self.show_info_msg("Porta serial %s aberta com sucesso!" % (self.avr.port))
						self.btnConnectDisconnect.setText("Desconectar")
						self.cbSerialPorts.setEnabled(False)
				except Exception as e:
					self.show_error_msg("Erro ao abrir a porta serial")
			else:
				try:
					if self.avr.close():
						self.show_info_msg("Porta serial %s fechada com sucesso!" % (self.avr.port))
						self.btnConnectDisconnect.setText("Conectar")
						self.cbSerialPorts.setEnabled(True)
						self.avr.serialPort = None
				except Exception as e:
					self.show_error_msg("Erro ao fechar a porta serial")

	def doStartStop(self):
		if not self.avr.acqThread.isAlive:
			try:
				self.avr.start()
				#Thread that handles the data acquisition
				self.dataProc = ThreadHandler(self.runAquisition)

				#Start the threads
				self.avr.acqThread.start()
				self.dataProc.start()

				self.btnStartStop.setText("Stop")
				self.btnConnectDisconnect.setEnabled(False)
			except Exception as e:
				self.show_error_msg("Erro ao iniciar aquisicao.\nError Log: " + str(e))
		else:
			try:
				self.doStop()
				self.btnStartStop.setText("Start")
				self.btnConnectDisconnect.setEnabled(True)
			except Exception as e:
				self.show_error_msg("Erro ao finalizar aquisicao.\nError Log: " + str(e))

	def doStop(self):
		self.avr.stop()
		time.sleep(1)
		#Kill the threads
		self.avr.acqThread.kill()
		self.dataProc.kill()

	def runAquisition(self):
		if self.avr.dataQueue.qsize() > 0:
			self.disassemblePacket()

	def disassemblePacket(self):
		n = self.avr.dataQueue.qsize()
		for i in range(n):
			data = self.avr.dataQueue.get()
			#print "data:\t%.2f\t%.2f" % (data[0], data[1])
			self.showEmgValues(data[0],data[1])

			if not self.ch0Contraido and data[0] > self.thresholdCh0:
				print 'Contraindo EMG0'
				self.ch0State = True
				self.stateChanged = True
			elif self.ch0Contraido and data[0] < self.thresholdCh0:
				print 'Relaxando EMG0'
				self.ch0State = False
				self.stateChanged = True

			if not self.ch1Contraido and data[1] > self.thresholdCh1:
				print 'Contraindo EMG1'
				self.ch1State = True
				self.stateChanged = True
			elif self.ch1Contraido and data[1] < self.thresholdCh1:
				print 'Relaxando EMG1'
				self.ch1State = False
				self.stateChanged = True

			self.runStateMachine()

			self.ch0Contraido = data[0] > self.thresholdCh0
			self.ch1Contraido = data[1] > self.thresholdCh1

	def runStateMachine(self):
		if self.stateChanged:
			self.stateChanged = False
			self.cbCh0.setCheckState(Qt.Checked if self.ch0State else Qt.Unchecked)
			self.cbCh1.setCheckState(Qt.Checked if self.ch1State else Qt.Unchecked)
			#self.cbCh0.setCheckState(self.ch0State)
			#State 1 1
			if  self.ch1State and  self.ch0State:
				fluidsynth.stop_Note(self.soundCommand1,0)
				fluidsynth.stop_Note(self.soundCommand2,0)
				fluidsynth.play_Note(self.soundCommand3,0,100)
				self.lbStatus.setText("Status: Comando 3")
				self.updadeGraphView(self.circleNota3)
			#State 0 1
			elif not self.ch1State and self.ch0State:
				fluidsynth.play_Note(self.soundCommand1,0,100)
				fluidsynth.stop_Note(self.soundCommand2,0)
				fluidsynth.stop_Note(self.soundCommand3,0)
				self.lbStatus.setText("Status: Comando 1")
				self.updadeGraphView(self.circleNota1)
			#State 1 0
			elif self.ch1State and not self.ch0State:
				fluidsynth.stop_Note(self.soundCommand1,0)
				fluidsynth.play_Note(self.soundCommand2,0,100)
				fluidsynth.stop_Note(self.soundCommand3,0)
				self.lbStatus.setText("Status: Comando 2")
				self.updadeGraphView(self.circleNota2)

			#State 1 1
			else:
				#fluidsynth.stop_everything()
				fluidsynth.stop_Note(self.soundCommand1,0)
				fluidsynth.stop_Note(self.soundCommand2,0)
				fluidsynth.stop_Note(self.soundCommand3,0)
				self.lbStatus.setText("Status: Silencio")
				self.updadeGraphView(self.circleSilence)




	def emgValueChanged(self):
		self.lbCh0.setText('CH0: %.2f V' % (self.sliderCh0.value()/100.00))
		self.lbCh1.setText('CH1: %.2f V' % (self.sliderCh1.value()/100.00))

	def thresholdChanged(self):
		self.lbThCh0.setText('Limiar CH0: %.2f V' % (self.sliderThCh0.value()/100.00))
		self.lbThCh1.setText('Limiar CH1: %.2f V' % (self.sliderThCh1.value()/100.00))
		self.thresholdCh0 = (self.sliderThCh0.value()/100.00)
		self.thresholdCh1 = (self.sliderThCh0.value()/100.00)

	def showEmgValues(self,valorCh0,valorCh1):
		self.sliderCh0.setValue(int(np.round(valorCh0*100)))
		self.sliderCh1.setValue(int(np.round(valorCh1*100)))

	def populateInstruments(self):
		for instrumento in self.ihmMIDIinstrument.names:
			self.cbInstruments.addItem(instrumento)
		self.cbInstruments.setCurrentIndex(0)

	def cbInstrumentsChanged(self, idx):
		fluidsynth.set_instrument(0, idx)

	def populateNotesAndScales(self):
		Notas = "C D E F G A B"
		for possivel_nota in Notas.split():
			self.cbNota1.addItem(possivel_nota)
			self.cbNota2.addItem(possivel_nota)
			self.cbNota3.addItem(possivel_nota)
		for possivel_nota in Notas.split():
			self.cbNota1.addItem(possivel_nota+"#")
			self.cbNota2.addItem(possivel_nota+"#")
			self.cbNota3.addItem(possivel_nota+"#")
		for possivel_nota in Notas.split():
			self.cbNota1.addItem(possivel_nota+"b")
			self.cbNota2.addItem(possivel_nota+"b")
			self.cbNota3.addItem(possivel_nota+"b")

		for n in range(0,9):
			self.cbEscala1.addItem(str(n))
			self.cbEscala2.addItem(str(n))
			self.cbEscala3.addItem(str(n))

			self.cbNota1.setCurrentIndex(0)
			self.cbNota2.setCurrentIndex(2)
			self.cbNota3.setCurrentIndex(4)

			self.cbEscala1.setCurrentIndex(4)
			self.cbEscala2.setCurrentIndex(4)
			self.cbEscala3.setCurrentIndex(4)

	def cbNotasChanged(self, idx):
		n1 = str(self.cbNota1.itemText(self.cbNota1.currentIndex()))
		n2 = str(self.cbNota2.itemText(self.cbNota2.currentIndex()))
		n3 = str(self.cbNota3.itemText(self.cbNota3.currentIndex()))

		e1 = self.cbEscala1.currentIndex()
		e2 = self.cbEscala2.currentIndex()
		e3 = self.cbEscala3.currentIndex()

		print "Notas: %s%d - %s%d - %s%d" % (n1,e1,n2,e2,n3,e3)

		self.soundCommand1 = Note(n1, e1)
		self.soundCommand2 = Note(n2, e2)
		self.soundCommand3 = Note(n3, e3)

	def updadeGraphView(self,shapetoupdate):
		self.circleSilence.setBrush(self.normalColor)
		self.circleNota1.setBrush(self.normalColor)
		self.circleNota2.setBrush(self.normalColor)
		self.circleNota3.setBrush(self.normalColor)
		shapetoupdate.setBrush(self.hilightColor)

if __name__ == '__main__':
	import sys
	from PyQt4 import QtGui

	app = QtGui.QApplication(sys.argv)
	main = Main()
	#main.plot()
	main.show()
	sys.exit(app.exec_())
