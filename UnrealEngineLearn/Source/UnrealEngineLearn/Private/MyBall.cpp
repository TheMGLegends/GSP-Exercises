// Fill out your copyright notice in the Description page of Project Settings.


#include "MyBall.h"
#include <Kismet/GameplayStatics.h>

// Sets default values
AMyBall::AMyBall()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

}

// Called when the game starts or when spawned
void AMyBall::BeginPlay()
{
	Super::BeginPlay();
	playerController = UGameplayStatics::GetPlayerController(GetWorld(), 0);
}

// Called every frame
void AMyBall::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

	InstantiateBall();
	if (playerController->WasInputKeyJustPressed(EKeys::B)) {
	}
}

FVector AMyBall::RandomPos()
{
	return FVector(FMath::RandRange(-1000, 1000), FMath::RandRange(-1000, 1000), FMath::RandRange(-1000, 1000));
}

void AMyBall::InstantiateBall()
{
	FActorSpawnParameters spawnParams;
	spawnParams.Owner = this;
	spawnParams.Instigator = GetInstigator();
	AActor* OurNewObject = GetWorld()->SpawnActor<AActor>(ourSpawningObject, RandomPos(), FRotator(0), spawnParams);
}

