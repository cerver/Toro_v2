MODULE MainModule

	! test04_001

	! variables
	TASK PERS tooldata drill := [FALSE, [[300,-260,280], [0,0,0,1]], [1,[0,0,0.001],[1,0,0,0],0,0,0]];
	TASK PERS wobjdata block := [TRUE, TRUE, "", [[0,0,150], [1,0,0,0]], [[0,0,0], [1,0,0,0]]];
	TASK PERS speeddata rate := [3, 500, 5000, 1000];

	! targets
	VAR jointtarget j0 := [[-90,0,0,90,90,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR jointtarget j1 := [[-41,0,0,0,90,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p00 := [[8.99826150854451E-31,120,-1.46952762458685E-14], [0.707106781186548,0.707106781186547,-2.16489014058873E-17,2.16489014058873E-17], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p01 := [[3.74927562856021E-31,50,-6.12303176911188E-15], [0.707106781186548,0.707106781186547,-2.16489014058873E-17,2.16489014058873E-17], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p02 := [[7.49855125712043E-32,10,-1.22460635382238E-15], [0.707106781186548,0.707106781186547,-2.16489014058873E-17,2.16489014058873E-17], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p10 := [[-1.70873387102589E-15,112.763114494309,41.0424171990802], [0.573576436351046,0.819152044288992,-1.19399581128633E-17,1.70520273794701E-17], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p11 := [[-7.11972446260788E-16,46.9846310392954,17.1010071662834], [0.573576436351046,0.819152044288992,-1.19399581128633E-17,1.70520273794701E-17], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p12 := [[-1.42394489252158E-16,9.39692620785909,3.42020143325669], [0.573576436351046,0.819152044288992,-1.19399581128633E-17,1.70520273794701E-17], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p20 := [[8.21055446514487E-16,91.9253331742774,77.1345131623847], [0.422618261740699,0.90630778703665,-3.68851074344187E-17,1.34250811938394E-17], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p21 := [[3.42106436047703E-16,38.3022221559489,32.139380484327], [0.422618261740699,0.90630778703665,-3.68851074344187E-17,1.34250811938394E-17], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p22 := [[6.84212872095406E-17,7.66044443118978,6.42787609686539], [0.422618261740699,0.90630778703665,-3.68851074344187E-17,1.34250811938394E-17], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p30 := [[3.33066907387547E-15,60.0000000000001,103.923048454133], [0.258819045102521,0.965925826289068,-2.68098273106551E-17,-7.18367157710789E-18], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p31 := [[1.38777878078145E-15,25,43.3012701892219], [0.258819045102521,0.965925826289068,-2.68098273106551E-17,-7.18367157710789E-18], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p32 := [[2.77555756156289E-16,5,8.66025403784438], [0.258819045102521,0.965925826289068,-2.68098273106551E-17,-7.18367157710789E-18], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p40 := [[1.15672923018016E-15,20.8377813200316,118.176930361465], [0.087155742747658,0.996194698091748,-2.76499572707741E-17,-2.41905780816892E-18], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p41 := [[4.81970512575065E-16,8.68240888334652,49.2403876506104], [0.087155742747658,0.996194698091748,-2.76499572707741E-17,-2.41905780816892E-18], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p42 := [[9.6394102515013E-17,1.7364817766693,9.84807753012208], [0.087155742747658,0.996194698091748,-2.76499572707741E-17,-2.41905780816892E-18], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p50 := [[-1.22902480706641E-15,-20.8377813200316,118.176930361465], [-2.57024892117948E-18,2.93780796001974E-17,0.996194698091746,0.087155742747658], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p51 := [[-5.12093669611005E-16,-8.6824088833465,49.2403876506104], [-2.57024892117948E-18,2.93780796001974E-17,0.996194698091746,0.087155742747658], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p52 := [[-1.02418733922201E-16,-1.7364817766693,9.84807753012208], [-2.57024892117948E-18,2.93780796001974E-17,0.996194698091746,0.087155742747658], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p60 := [[-7.43422259608843E-15,-60,103.923048454133], [-3.0401663099209E-17,6.22124207821965E-18,0.965925826289068,0.258819045102521], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p61 := [[-3.09759274837018E-15,-25,43.3012701892219], [-3.0401663099209E-17,6.22124207821965E-18,0.965925826289068,0.258819045102521], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p62 := [[-6.19518549674036E-16,-5,8.66025403784439], [-3.0401663099209E-17,6.22124207821965E-18,0.965925826289068,0.258819045102521], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p70 := [[8.56365125061317E-15,-91.9253331742773,77.1345131623847], [5.03101886282581E-17,2.34600262405793E-17,0.90630778703665,0.422618261740699], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p71 := [[3.56818802108882E-15,-38.3022221559489,32.139380484327], [5.03101886282581E-17,2.34600262405793E-17,0.90630778703665,0.422618261740699], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p72 := [[7.13637604217765E-16,-7.66044443118978,6.4278760968654], [5.03101886282581E-17,2.34600262405793E-17,0.90630778703665,0.422618261740699], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p80 := [[2.27831182803452E-15,-112.763114494309,41.0424171990802], [2.27360365059601E-17,1.59199441504844E-17,0.819152044288992,0.573576436351046], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p81 := [[9.49296595014383E-16,-46.9846310392954,17.1010071662834], [2.27360365059601E-17,1.59199441504844E-17,0.819152044288992,0.573576436351046], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p82 := [[1.89859319002877E-16,-9.39692620785909,3.42020143325669], [2.27360365059601E-17,1.59199441504844E-17,0.819152044288992,0.573576436351046], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p90 := [[0,-120,0], [2.16489014058873E-17,2.16489014058873E-17,0.707106781186547,0.707106781186548], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p91 := [[0,-50,0], [2.16489014058873E-17,2.16489014058873E-17,0.707106781186547,0.707106781186548], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p92 := [[0,-10,0], [2.16489014058873E-17,2.16489014058873E-17,0.707106781186547,0.707106781186548], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p100 := [[-1.70873387102589E-15,-112.763114494309,-41.0424171990802], [1.19399581128633E-17,1.70520273794701E-17,0.573576436351046,0.819152044288992], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p101 := [[-7.11972446260788E-16,-46.9846310392954,-17.1010071662834], [1.19399581128633E-17,1.70520273794701E-17,0.573576436351046,0.819152044288992], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p102 := [[-1.42394489252158E-16,-9.39692620785909,-3.42020143325669], [1.19399581128633E-17,1.70520273794701E-17,0.573576436351046,0.819152044288992], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p110 := [[2.27831182803452E-15,112.763114494309,-41.0424171990803], [0.819152044288992,0.573576436351046,-2.27360365059601E-17,1.59199441504844E-17], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p111 := [[9.49296595014384E-16,46.9846310392954,-17.1010071662834], [0.819152044288992,0.573576436351046,-2.27360365059601E-17,1.59199441504844E-17], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];
	VAR robtarget p112 := [[1.89859319002877E-16,9.39692620785908,-3.42020143325669], [0.819152044288992,0.573576436351046,-2.27360365059601E-17,1.59199441504844E-17], [0,0,0,0], [9E9,9E9,9E9,9E9,9E9,9E9]];

	! drilling instructions
	PROC main()
		ConfL\Off;
		SingArea\Wrist;

		TPWrite("This is: test04_001");
		TPWrite("Check block and drill");
		MoveAbsJ j0, v100, z5, tool0;
		MoveAbsJ j1, v100, z5, tool0;

		TPWrite("Drilling hole 1 of 12!");
		MoveL p00, v100, z30, drill\WObj:=block;
		MoveL p01, v100, z5, drill\WObj:=block;
		MoveL p02, rate, fine, drill\WObj:=block;
		MoveL p01, rate, fine, drill\WObj:=block;
		MoveL p00, v100, z30, drill\WObj:=block;
		MoveL RelTool(p00, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p10, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 2 of 12!");
		MoveL p10, v100, z30, drill\WObj:=block;
		MoveL p11, v100, z5, drill\WObj:=block;
		MoveL p12, rate, fine, drill\WObj:=block;
		MoveL p11, rate, fine, drill\WObj:=block;
		MoveL p10, v100, z30, drill\WObj:=block;
		MoveL RelTool(p10, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p20, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 3 of 12!");
		MoveL p20, v100, z30, drill\WObj:=block;
		MoveL p21, v100, z5, drill\WObj:=block;
		MoveL p22, rate, fine, drill\WObj:=block;
		MoveL p21, rate, fine, drill\WObj:=block;
		MoveL p20, v100, z30, drill\WObj:=block;
		MoveL RelTool(p20, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p30, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 4 of 12!");
		MoveL p30, v100, z30, drill\WObj:=block;
		MoveL p31, v100, z5, drill\WObj:=block;
		MoveL p32, rate, fine, drill\WObj:=block;
		MoveL p31, rate, fine, drill\WObj:=block;
		MoveL p30, v100, z30, drill\WObj:=block;
		MoveL RelTool(p30, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p40, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 5 of 12!");
		MoveL p40, v100, z30, drill\WObj:=block;
		MoveL p41, v100, z5, drill\WObj:=block;
		MoveL p42, rate, fine, drill\WObj:=block;
		MoveL p41, rate, fine, drill\WObj:=block;
		MoveL p40, v100, z30, drill\WObj:=block;
		MoveL RelTool(p40, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p50, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 6 of 12!");
		MoveL p50, v100, z30, drill\WObj:=block;
		MoveL p51, v100, z5, drill\WObj:=block;
		MoveL p52, rate, fine, drill\WObj:=block;
		MoveL p51, rate, fine, drill\WObj:=block;
		MoveL p50, v100, z30, drill\WObj:=block;
		MoveL RelTool(p50, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p60, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 7 of 12!");
		MoveL p60, v100, z30, drill\WObj:=block;
		MoveL p61, v100, z5, drill\WObj:=block;
		MoveL p62, rate, fine, drill\WObj:=block;
		MoveL p61, rate, fine, drill\WObj:=block;
		MoveL p60, v100, z30, drill\WObj:=block;
		MoveL RelTool(p60, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p70, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 8 of 12!");
		MoveL p70, v100, z30, drill\WObj:=block;
		MoveL p71, v100, z5, drill\WObj:=block;
		MoveL p72, rate, fine, drill\WObj:=block;
		MoveL p71, rate, fine, drill\WObj:=block;
		MoveL p70, v100, z30, drill\WObj:=block;
		MoveL RelTool(p70, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p80, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 9 of 12!");
		MoveL p80, v100, z30, drill\WObj:=block;
		MoveL p81, v100, z5, drill\WObj:=block;
		MoveL p82, rate, fine, drill\WObj:=block;
		MoveL p81, rate, fine, drill\WObj:=block;
		MoveL p80, v100, z30, drill\WObj:=block;
		MoveL RelTool(p80, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p90, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 10 of 12!");
		MoveL p90, v100, z30, drill\WObj:=block;
		MoveL p91, v100, z5, drill\WObj:=block;
		MoveL p92, rate, fine, drill\WObj:=block;
		MoveL p91, rate, fine, drill\WObj:=block;
		MoveL p90, v100, z30, drill\WObj:=block;
		MoveL RelTool(p90, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p100, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 11 of 12!");
		MoveL p100, v100, z30, drill\WObj:=block;
		MoveL p101, v100, z5, drill\WObj:=block;
		MoveL p102, rate, fine, drill\WObj:=block;
		MoveL p101, rate, fine, drill\WObj:=block;
		MoveL p100, v100, z30, drill\WObj:=block;
		MoveL RelTool(p100, 0, 50, 0), v100, z5, drill\WObj:=block;
		MoveAbsJ CalcJointT(RelTool(p110, 0, 50, 0), drill\WObj:=block), v100, z5, drill\WObj:=block;

		TPWrite("Drilling hole 12 of 12!");
		MoveL p110, v100, z30, drill\WObj:=block;
		MoveL p111, v100, z5, drill\WObj:=block;
		MoveL p112, rate, fine, drill\WObj:=block;
		MoveL p111, rate, fine, drill\WObj:=block;
		MoveL p110, v100, z30, drill\WObj:=block;
		MoveL RelTool(p110, 0, 50, 0), v100, z5, drill\WObj:=block;

		TPWrite("Resetting axes...");
		MoveAbsJ j1, v100, z5, tool0;
		MoveAbsJ j0, v100, z5, tool0;

		Stop;
	ENDPROC

ENDMODULE