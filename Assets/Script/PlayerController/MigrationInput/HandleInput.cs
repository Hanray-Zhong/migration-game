﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleInput : MigrationInput {

	private float lastInput = 0;

	public override Vector2 GetInputAxis() {
		Vector2 moveDir = new Vector2(0, 0);
		if (Input.GetAxisRaw("HorizontalHandle") > 0.3) {
			moveDir = new Vector2(1, -1).normalized;
		}
		if (Input.GetAxisRaw("HorizontalHandle") < -0.3) {
			moveDir = new Vector2(-1, 1).normalized;
		}
		if (Input.GetAxisRaw("VerticalHandle") > 0.3f) {
			moveDir = new Vector2(1, 1).normalized;
		}
		if (Input.GetAxisRaw("VerticalHandle") < -0.3f) {
			moveDir = new Vector2(-1, -1).normalized;
		}
		if (Input.GetAxisRaw("HorizontalHandle") > 0 && Input.GetAxisRaw("VerticalHandle") > 0) {
			moveDir = new Vector2(1, 0).normalized;
		}
		if (Input.GetAxisRaw("HorizontalHandle") > 0 && Input.GetAxisRaw("VerticalHandle") < -0) {
			moveDir = new Vector2(0, -1).normalized;
		}
		if (Input.GetAxisRaw("HorizontalHandle") < -0 && Input.GetAxisRaw("VerticalHandle") > 0) {
			moveDir = new Vector2(0, 1).normalized;
		}
		if (Input.GetAxisRaw("HorizontalHandle") < -0 && Input.GetAxisRaw("VerticalHandle") < -0) {
			moveDir = new Vector2(-1, 0).normalized;
		}
		if (Input.GetAxisRaw("HorizontalHandle") == 0 && Input.GetAxisRaw("VerticalHandle") == 0) {
			moveDir = new Vector2(0, 0);
		}
		return moveDir;
	}

	public override bool GetInputInteraction() {
		bool GetButtonDown;
		if (Input.GetAxis( "Button_A" ) > 0.2 && lastInput == 0)
			GetButtonDown = true;
		else
			GetButtonDown = false;
		lastInput = Input.GetAxis("Button_A");
		return GetButtonDown;
	}
	public override bool OpenTechTree() {
		bool GetButtonDown;
		if (Input.GetAxis( "Button_Y" ) > 0.2 && lastInput == 0)
			GetButtonDown = true;
		else
			GetButtonDown = false;
		lastInput = Input.GetAxis("Button_Y");
		return GetButtonDown;
	}
	public override bool CloseTechTree() {
		bool GetButtonDown;
		if (Input.GetAxis( "Button_B" ) > 0.2 && lastInput == 0)
			GetButtonDown = true;
		else
			GetButtonDown = false;
		lastInput = Input.GetAxis("Button_B");
		return GetButtonDown;
	}
}